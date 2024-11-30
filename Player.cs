public abstract class Player{
    public string name;
    public Player(string name)
    {
        this.name = name;
    }
    
    public List<Ficha> fichas = new List<Ficha>();
}

public class NewPlayer : Player
{
    public NewPlayer(string name) : base(name)
    {
    }
}
