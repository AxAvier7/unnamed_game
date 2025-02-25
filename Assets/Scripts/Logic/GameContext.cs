using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour//este script guarda el estado del juego para que ciertos parametros sea accesibles
{
    public static GameContext Instance { get; private set; }
    public Casilla[,] maze;
    public List<Player> players;
    public int turn;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        players = new List<Player>();
        turn = 0;
    }
}
