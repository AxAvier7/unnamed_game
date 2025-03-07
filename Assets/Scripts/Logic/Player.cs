using System.Collections.Generic;

public abstract class Player //clase abstracta que define a los jugadores
{
    public string name;    
    public List<Ficha> fichas;
    public bool isInvisible;

    public Player(string name)
    {
        this.name = name;
        isInvisible = false;
        fichas = new List<Ficha>();
    }
}

public class NewPlayer : Player
{
    public NewPlayer(string name) : base(name){}
}