using System.Collections.Generic;

public abstract class Player
{
    public string name;    
    public List<Ficha> fichas;
    public Player(string name)
    {
        this.name = name;
        fichas = new List<Ficha>();
    }

    public void AddFicha(Ficha ficha)
    {
        fichas.Add(ficha);
    }
    
}

public class NewPlayer : Player
{
    public NewPlayer(string name) : base(name){}
}