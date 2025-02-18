using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapManager : MonoBehaviour 
{
    public static TrapManager Instance;
    public GameObject trapEffectPanel;
    public Text trapMessageText;
    public float effectDuration = 3f;
    public Dictionary<Vector2Int, CasillaTrampa> trampas = new Dictionary<Vector2Int, CasillaTrampa>();
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegistrarTrampa(Vector2Int coordenadas, CasillaTrampa trampa)
    {
        if (!trampas.ContainsKey(coordenadas))
        {
            trampas.Add(coordenadas, trampa);
        }
    }

    public bool HayTrampaEn(Vector2Int coordenadas, out CasillaTrampa trampa)
    {
        return trampas.TryGetValue(coordenadas, out trampa);
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