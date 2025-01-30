public abstract class Trampa
{
    public string name;
    public string description;
    public int modifier;
    public bool isTriggered;
    
    public Trampa(string name, string description, int modifier)
    {
        this.name = name;
        this.description = description;
        this.modifier = modifier;
    }
    public abstract void Trap();
}