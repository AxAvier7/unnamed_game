using System;
using System.Collections.Generic;

public class MazeGenerator
{
    private int[,] maze;
    private int width, height;
    private Random random = new Random();

    //las casillas con 0 son paredes, las casillas con 1 son caminos
    public MazeGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
        maze = new int[height, width];
    }

    //metodo para generar el laberinto
    public int[,] GenerateMaze((int x, int y) start, (int x, int y) end)
    {
        //inicializo todas las casillas como paredes excepto la entrada y la salida
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                maze[i, j] = IsBorder(j,i) ? 0 : 0;
            }
        }
        GeneratePath(start.x, start.y, end);
        EnsureConnection(end);

        maze[start.y, start.x] = 1;
        maze[end.y, end.x] = 1;
        return maze;
    }

    //metodo que traza un camino
    private bool GeneratePath(int x, int y, (int x, int y) end)
    {
        maze[y, x] = 1;
        if (x == end.x && y == end.y) return true;

        //todas las posibles direcciones
        var directions = new List<(int dx, int dy)> { 
            (Math.Sign(end.x - x), 0), (0, Math.Sign(end.y - y)), 
            (-1, 0), (1, 0),
            (0, -1), (0, 1) 
        };
        Shuffle(directions);

        //probamos el movimiento en cada direccion
        foreach (var (dx, dy) in directions)
        {
            //revisamos dos casillas mas adelante de la direccion en la que estemos
            int nx = x + 2 * dx;
            int ny = y + 2 * dy;

            //revisa si la casilla esta en los limites de la matriz, no es un borde y esta sin recorrer
            if (IsInBounds(nx, ny) && maze[ny, nx] == 0 && !IsBorder(nx, ny))
            {
                maze[y + dy, x + dx] = 1;

                //llama recursivamente al metodo para seguir generando el camino
                if(GeneratePath(nx, ny, end)) return true;
            }
        }
        return false;
    }

    //metodo que revisa si la casilla de salida esta conectada con el resto
    private void EnsureConnection((int x, int y) end)
    {
        if(maze[end.y, end.x] == 1) return;

        var directions = new List<(int dx, int dy)>
        {(-1, 0), (1, 0), (0, -1), (0, 1)};

        //si la salida no está conectada busca casillas cercanas transitables para conectarla
        foreach (var (dx, dy) in directions)
        {
            int nx = end.x + dx;
            int ny = end.y + dy;

            if (IsInBounds(nx, ny) && !IsBorder(nx, ny))
            {
                maze[ny, nx] = 1;
                maze[end.y, end.x] = 1;
                return;
            }
        }

        //de no encuentrar un camino cercano, crea uno
        GeneratePath(end.x, end.y, end);    
    }

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    //Metodo que verifica si (x,y) esta dentro de los limites del laberinto
    private bool IsInBounds(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

    //Metodo que verifica que (x,y) sea un borde
    private bool IsBorder(int x, int y) => x == 0 || x == width - 1 || y == 0 || y == height - 1;
}
