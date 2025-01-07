using UnityEngine;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public Transform mazeGrid;

    void Start()
    {
        GenerateMazeFromGameData();
    }

    void GenerateMazeFromGameData()
    {
        int players = GameData.Instance.Players;
        int chips = GameData.Instance.Chips;

        int[,] maze = MazeGen(players, chips);
        VisualizeMaze(maze);
    }


    static int[,] MazeGen(int players, int chips) //metodo que genera el laberinto en el que se jugara
    {
        int size = players*chips >= 12 ? 12 : 11;
        int[,] maze = new int[size,size];
        var (x1,y1,x2,y2) = CoordinatesRandomizer(maze);
        MazeGenerator generator = new MazeGenerator(size, size);
        maze = generator.GenerateMaze((x1, y1), (x2, y2));
        return maze;
    }


    static (int x1, int y1, int x2, int y2) CoordinatesRandomizer(int[,] maze) //metodo que recibe la matriz del laberinto y devuelve la misma matriz pero con una entrada y salida marcadas
    {    
        int rows = maze.GetLength(0);
        int columns = maze.GetLength(1);
        int x1 =0, y1 = 0, x2 = 0, y2 = 0;
        System.Random rng = new System.Random();
            
        int border = rng.Next(0,4); // 0: 1ra fila | 1: ultima fila | 2: 1ra columna | 3: ultima columna
        int border2 = rng.Next(0,4);

        switch (border) //decidimos las coordenadas de la entrada al laberinto
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

    void VisualizeMaze(int[,] maze)
    {
        foreach (Transform child in mazeGrid)
        {
            Destroy(child.gameObject);
        }

        RectTransform gridRectTransform = mazeGrid.GetComponent<RectTransform>();
        float cellSize = gridRectTransform.rect.width / maze.GetLength(1);
        var gridLayoutGroup = mazeGrid.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        for (int y = 0; y < maze.GetLength(0); y++)
        {
            for (int x = 0; x < maze.GetLength(1); x++)
            {
                GameObject prefab = maze[y, x] == 1 ? pathPrefab : wallPrefab;
                GameObject cell = Instantiate(prefab, mazeGrid);

            }
        }
    }
}