using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour //clase donde se guardan los parametros que se usan para generar el laberinto y que estos no se pierdan de una escena a otra
{
    public static GameData Instance { get; private set; }
    public int Players { get; set; }
    public int Chips { get; set; }
    public List<string> PlayerNames { get; private set; } = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}