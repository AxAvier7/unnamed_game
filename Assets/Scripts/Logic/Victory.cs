using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour //este script muestra un mensaje de victoria y carga la inicial del juego
{
    public static Victory Instance;
    
    public GameObject victoryPanel;
    public Text victoryText;
    private float delayBeforeRestart = 5f;

    private void Awake()

    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowVictory(string playerName)
    {
        victoryPanel.SetActive(true);
        victoryText.text = $"{playerName} ha escapado del laberinto!";
        Invoke(nameof(ReturnToMainMenu), delayBeforeRestart);
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}