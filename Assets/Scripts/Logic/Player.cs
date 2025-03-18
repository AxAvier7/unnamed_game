using System.Collections.Generic;

public abstract class Player //clase abstracta que define a los jugadores
{
    public string name;    
    public List<Ficha> fichas;
    public int turnosRestantesInmunidad;
    public bool isParalized;

    public Player(string name)
    {
        this.name = name;
        turnosRestantesInmunidad = 0;
        fichas = new List<Ficha>();
    }
}

public class NewPlayer : Player
{
    public NewPlayer(string name) : base(name){}
}