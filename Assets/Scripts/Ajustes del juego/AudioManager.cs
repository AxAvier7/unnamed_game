using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class Sound
{
    public string nombre;
    public string artista;
    public string persona;
    public Sprite portada;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 0.7f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop;
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] sonidos;
    public static AudioManager instance;

    [Header("Playlist")]
    public List<string> playlist = new List<string>();
    private int currentSongIndex = -1;
    private Sound currentSound;

    public Text songTitleText;
    public Text songInfoText;
    public Image songCoverImage;
    public float notificationDuration = 5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sonidos)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        SetUIVisibility(false);
        if (playlist.Count > 0)
        {
            PlayRandomFromPlaylist();
        }
        else
        {
            Debug.LogWarning("La playlist está vacía. Añade canciones a la playlist en el inspector.");
        }
    }

    void Update()
    {
        if (currentSound?.source?.isPlaying == false)
        {
            PlayNextFromPlaylist();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sonidos, sound => sound.nombre == name);
        if (s == null)
        {
            Debug.LogWarning($"El sonido '{name}' no existe en la lista de sonidos.");
            return;
        }

        if (currentSound != null && currentSound.source.isPlaying)
        {
            currentSound.source.Stop();
        }

        s.source.Play();
        currentSound = s;

        UpdateUI(s);
    }

    private void UpdateUI(Sound s)
    {

        if (songTitleText != null) songTitleText.text = $"Playing: {s.nombre}";
        if (songInfoText != null) songInfoText.text = $"By {s.artista}. Given by: {s.persona}";
        if (songCoverImage != null) songCoverImage.sprite = s.portada;

        SetUIVisibility(true);
        StopAllCoroutines();
        StartCoroutine(HideNotificationAfterDelay());
    }

    private void SetUIVisibility(bool isVisible)
    {
        if (songTitleText != null) songTitleText.gameObject.SetActive(isVisible);
        if (songInfoText != null) songInfoText.gameObject.SetActive(isVisible);
        if (songCoverImage != null) songCoverImage.gameObject.SetActive(isVisible);
    }

    private IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSeconds(notificationDuration);
        SetUIVisibility(false);
    }

    public void AddToPlaylist(string name)
    {
        if (Array.Exists(sonidos, sound => sound.nombre == name) && !playlist.Contains(name))
        {
            playlist.Add(name);
        }
    }

    public void PlayNextFromPlaylist()
    {
        if (playlist.Count == 0) return;

        currentSongIndex = (currentSongIndex + 1) % playlist.Count;

        string nextSong = playlist[currentSongIndex];
        Play(nextSong);
    }

    public void PlayRandomFromPlaylist()
    {
        if (playlist.Count == 0) return;

        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, playlist.Count);
        } while (randomIndex == currentSongIndex && playlist.Count > 1);

        currentSongIndex = randomIndex;
        Play(playlist[randomIndex]);
    }
}
