using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Buenas querido usuario. Cuántos aventureros serán hoy?");
        int players = GetPlayers();
        
        Console.WriteLine("Ya veo. Yo seré su \"guía\" en esta aventura. Si les preguntara cuales son vuestros nombres, que me responderían?");
        List<NewPlayer> playerlist = GetPlayerNames(players);
        
        Console.WriteLine("Vale, ultimemos algunos detalles. Con cuántas fichas desean jugar?");
        int chips = GetChips();

        int[,] playinglab = MazeGenerator(players, chips);
        Utils.PrintMatrix(playinglab);

        //Fichas disponibles
        var ficha1 = new CooldownChip(2, 3, "Una ficha que reduce el cooldown");
        var ficha2 = new SpeedChip(2, 2, "Una ficha que aumenta su velocidad");
        var ficha3 = new NormieChip(1, 1, "La ficha mas basica y versatil que puede haber");
        var ficha4 = new NormieChip(3, 2, "Una ficha normal pero con un cooldown bajo");

    }

    static int GetPlayers()
    {
        int players = Convert.ToInt32(Console.ReadLine());
        if(players > 4 || players == 1)
        {
            Console.WriteLine("No pueden haber más de 4 jugadores. Introduce un número de jugadores menor a 4 y distinto de 1");
            players = Convert.ToInt32(Console.ReadLine());
        }
        return players;
    }

    static List<NewPlayer> GetPlayerNames(int players)
    {
        List<NewPlayer> playerlist = new List<NewPlayer>();
        for (int i = 0; i < playerlist.Count; i++)
        {
            Console.WriteLine($"Jugador {i + 1}, ingresa tu nombre:");
            string playerName = Console.ReadLine();
            playerlist.Add(new NewPlayer(playerName));
        }
        return playerlist;
    }

    static int GetChips()
    {
        int chips = Convert.ToInt32(Console.ReadLine());
        while (chips>5)
        {
            Console.WriteLine("Debido a cómo funcionan nuestros mazos, no admitimos que los jugadores tengan más de 5 fichas. Introduce una cantidad de fichas menor a 5");
            chips = Convert.ToInt32(Console.ReadLine());
        }
        return chips;
    }

    static int[,] MazeGenerator(int players, int chips) //metodo que genera el laberinto en el que se jugara
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
        Random rng = new Random();
            
        int border = rng.Next(0,4); // 0: 1ra fila | 1: ultima fila | 2: 1ra columna | 3: ultima columna
        int border2 = rng.Next(0,4);

        switch (border) //decidimos las coordenadas de la entrada al laberinto
        {
            case 0:
                x1 = 0;
                y1 = rng.Next(1, columns-2);
                break;
            case 1:
                x1 = rows - 1;
                y1 = rng.Next(1, columns - 2);
                break;
            case 2:
                x1 = rng.Next(1, rows - 2);
                y1 = 0;
                break;
            case 3:
                x1 = rng.Next(1, rows - 2);
                y1 = columns - 1;
                break;
            default:
                break;
        }

        while(border2 == border)
            border2 = rng.Next(0,4);

        switch (border2) //decidimos las coordenadas de la salida del laberinto
        {
            case 0:
                x2 = 0;
                y2 = rng.Next(1, columns-2);
                break;
            case 1:
                x2 = rows - 1;
                y2 = rng.Next(1, columns - 2);
                break;
            case 2:
                x2 = rng.Next(1, rows - 2);
                y2 = 0;
                break;
            case 3:
                x2 = rng.Next(1, rows - 2);
                y2 = columns - 1;
                break;
            default:
                break;
        }

        return (x1, y1, x2, y2);
    }
}