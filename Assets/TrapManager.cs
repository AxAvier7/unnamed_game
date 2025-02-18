using UnityEngine;
using UnityEngine.UI;

public class TrapManager : MonoBehaviour 
{
    public static TrapManager Instance;

    public GameObject trapEffectPanel;
    public Text trapMessageText;
    public float effectDuration = 3f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowTrapEffect(string message)
    {
        trapEffectPanel.SetActive(true);
        trapMessageText.text = message;
        Invoke(nameof(HideTrapEffect), effectDuration);
    }

    private void HideTrapEffect()
    {
        trapEffectPanel.SetActive(false);
    }
}