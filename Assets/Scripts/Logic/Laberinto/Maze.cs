﻿using UnityEngine;
using UnityEngine.UI;
using static CasillaTrampa;

public class Maze : MonoBehaviour //script donde se genera la parte visual del laberinto
{
    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public GameObject trapPrefab;
    public Transform mazeGrid;
    private Casilla[,] maze;

    void Start()
    {
        GenerateMazeFromGameData();
    }

    void GenerateMazeFromGameData()
    {
        int players = GameData.Instance.Players;
        int chips = GameData.Instance.Chips;
        maze = MazeGen(players, chips);
        GameContext.Instance.maze = maze;
        VisualizeMaze(maze);
    }

    Casilla[,] MazeGen(int players, int chips) //metodo que genera el laberinto en el que se jugara
    {
        int size = players*chips >= 12 ? 12 : 11;
        int currentTraps=0;
        Casilla[,] maze = new Casilla[size,size];
        var (x1,y1,x2,y2) = CoordinatesRandomizer(size, size);
        MazeGenerator generator = new MazeGenerator(size, size);
        int[,] generatedMaze = generator.GenerateMaze((x1, y1), (x2, y2));
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                bool esTransitable = generatedMaze[x, y] == 1;
                bool esInicio = (x == x1 && y == y1);
                bool esSalida = (x == x2 && y == y2);
                bool esTrampa = false;
                if(!esInicio && !esSalida && esTransitable && currentTraps<3)
                {
                    if(UnityEngine.Random.Range(0,5) == 1)
                    {
                        esTrampa = true;
                        currentTraps++;
                    }
                }
                if(esTrampa)
                {
                    GameObject trapCell = Instantiate(trapPrefab, mazeGrid);
                    CasillaTrampa casillaTrampa = trapCell.GetComponent<CasillaTrampa>();
                    casillaTrampa.Coordenadas = new Vector2Int(x, y);
                    casillaTrampa.EsTransitable = true;
                    casillaTrampa.efectoTrampa = CasillaTrampa.TipoEfectoTrampa.RegresarEntrada;
                    maze[x, y] = casillaTrampa;                
                }
                else
                {
                    maze[x, y] = new Casilla(new Vector2Int(x, y), esTransitable, esTrampa: esTrampa, esInicio: esInicio, esSalida: esSalida);
                }
            }
        }
        return maze;
    }
    static (int x1, int y1, int x2, int y2) CoordinatesRandomizer(int rows, int columns) //metodo que recibe la matriz del laberinto y devuelve la misma matriz pero con una entrada y salida marcadas
    {    

        int x1 =0, y1 = 0, x2 = 0, y2 = 0;
        System.Random rng = new System.Random();
            
        int border = rng.Next(0,4); // 0: 1ra fila | 1: ultima fila | 2: 1ra columna | 3: ultima columna
        int border2 = rng.Next(0,4);

        switch(border) //decidimos las coordenadas de la entrada al laberinto
        {
            case 0:
                x1 = 0;     y1 = rng.Next(1, columns-2);
                break;
            case 1:
                x1 = rows - 1;      y1 = rng.Next(1, columns - 2);
                break;
            case 2:
                x1 = rng.Next(1, rows - 2);     y1 = 0;
                break;
            case 3:
                x1 = rng.Next(1, rows - 2);     y1 = columns - 1;
                break;
        }

        while(border2 == border)
            border2 = rng.Next(0,4);

        switch (border2) //decidimos las coordenadas de la salida del laberinto
        {
            case 0:
                x2 = 0;     y2 = rng.Next(1, columns-2);
                break;
            case 1:
                x2 = rows - 1;      y2 = rng.Next(1, columns - 2);
                break;
            case 2:
                x2 = rng.Next(1, rows - 2);     y2 = 0;
                break;
            case 3:
                x2 = rng.Next(1, rows - 2);     y2 = columns - 1;
                break;
        }
        return (x1, y1, x2, y2);
    }

    void VisualizeMaze(Casilla[,] maze)//metodo con el que se muestra en la escena de Juego el laberinto
    {
        TrapManager.Instance.trampas.Clear();
        foreach(Transform child in mazeGrid)
        {
            Destroy(child.gameObject);
        }

        RectTransform gridRectTransform = mazeGrid.GetComponent<RectTransform>();
        float cellSize = gridRectTransform.rect.width / maze.GetLength(1);
        var gridLayoutGroup = mazeGrid.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        for(int x = 0; x < maze.GetLength(0); x++)
        {
            for(int y = 0; y < maze.GetLength(1); y++)
            {
                Casilla casilla = maze[x, y];
                GameObject prefab;

                if(casilla is CasillaTrampa)
                {
                    prefab = trapPrefab;
                    GameObject trapCell = Instantiate(prefab, mazeGrid);
                    CasillaTrampa casillaTrampa = trapCell.GetComponent<CasillaTrampa>();
                    TipoEfectoTrampa[] efectosTrampas = {TipoEfectoTrampa.MultiplicarCooldown, TipoEfectoTrampa.DividirVelocidad, TipoEfectoTrampa.RegresarEntrada, TipoEfectoTrampa.Teletransportar};
                    System.Random rng = new System.Random();
                    casillaTrampa.efectoTrampa = efectosTrampas[rng.Next(efectosTrampas.Length)];
                    casillaTrampa.Coordenadas = new Vector2Int(x, y);
                    casillaTrampa.EsTransitable = casilla.EsTransitable;
                    TrapManager.Instance.RegistrarTrampa(new Vector2Int(x, y), casillaTrampa);
                    trapCell.GetComponent<Image>().color = Color.magenta;
                    Debug.Log($"Casilla trampa en ({x}, {y})");

                }
                else
                {
                    prefab = casilla.EsTransitable ? pathPrefab : wallPrefab;
                    GameObject cell = Instantiate(prefab, mazeGrid);
                    cell.GetComponent<Casilla>().Coordenadas = new Vector2Int(x, y);
                    Casilla casillaVisual = cell.GetComponent<Casilla>();
                    casillaVisual.Coordenadas = new Vector2Int(x, y);
                    casillaVisual.EsInicio = casilla.EsInicio;
                    casillaVisual.EsSalida = casilla.EsSalida;
                    casillaVisual.EsTransitable = casilla.EsTransitable;

                    if(casilla.EsInicio)
                    {
                        cell.GetComponent<Image>().color = Color.red;
                        Debug.Log($"Casilla de inicio en ({x}, {y})");
                    }
                    else if(casilla.EsSalida)
                    {
                        cell.GetComponent<Image>().color = Color.green;
                        Debug.Log($"Casilla de salida en ({x}, {y})");
                    }
                }
            }
        }
    }
}