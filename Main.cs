Console.WriteLine("Buenas querido usuario. Cuántos aventureros serán hoy?");
int players = Convert.ToInt32(Console.ReadLine());
if(players > 4 || players == 1)
{
    Console.WriteLine("No pueden haber mas de 4 jugadores. Introduce un número de jugadores menor a 4 y distinto de 1");
    players = Convert.ToInt32(Console.ReadLine());
}

Console.WriteLine("Ya veo. Yo seré su \"guía\" en esta aventura. Si les preguntara cuales son vuestros nombres, que me responderían?");
string[] playernames = new string[players];
for (int i = 0; i < playernames.Length; i++)
{
    playernames[i] = Console.ReadLine();
}

Console.WriteLine("Vale, ultimemos algunos detalles. Con cuántas fichas desean jugar?");
int chips = Convert.ToInt32(Console.ReadLine());
while (chips>5)
{
    Console.WriteLine("Debido a cómo funcionan nuestros mazos, no admitimos que los jugadores tengan más de 5 fichas. Introduce una cantidad de fichas menor a 5");
    chips = Convert.ToInt32(Console.ReadLine());
}




int[,] playinglab = MazeGenerator(players, chips);
PrintMatrix(playinglab);





int[,] MazeGenerator(int players, int chips) //metodo que genera el laberinto en el que se jugara
{
    int size;
    int playersxchips = players*chips;

    size= playersxchips >=12? 9 : 8;

    int[,] maze = new int[size,size];

    var (x1,y1,x2,y2) = CoordinatesRandomizer(maze);
    bool[,] walls = new bool[size,size];
    walls[x1,y1] = walls[x2,y2] = true;

    walls = PathFinder(walls, x1, y1, x2, y2);
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if(walls[i,j])
            {
                maze[i,j] = 1;
            }
        }
    }



    return maze;
}


(int x1, int y1, int x2, int y2) CoordinatesRandomizer(int[,] maze) //metodo que recibe la matriz del laberinto y devuelve la misma matriz pero con una entrada y salida marcadas
{    
    int rows = maze.GetLength(0);
    int columns = maze.GetLength(1);
    bool[,] booleanmaze = new bool[rows,columns];
    int x1 =0,  y1 = 0, x2 = 0, y2 = 0;
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

bool[,] PathFinder(bool[,] walls, int x1, int y1, int x2, int y2)
{
    throw new NotImplementedException();
}


void PrintMatrix<T>(T[,] laberinto)
{
    int rows = laberinto.GetLength(0);
    int columns = laberinto.GetLength(1);

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < columns; j++)
        {
            Console.Write(laberinto[i, j] + "\t");
        }
        Console.WriteLine();
    }
}




var ficha1 = new FichaPrototype(2, 5, "No soy una ficha");
ficha1.Skill();
ficha1.MostrarInformacion();

//despues me pondre a crear mas fichas