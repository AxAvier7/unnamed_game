using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
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

    public void SetMaze(Casilla[,] newMaze)
    {
        maze = newMaze;
    }
}
