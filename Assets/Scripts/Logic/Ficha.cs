using System;

public abstract class Ficha //esta es la clase que define a las fichas basicas. Con esta creare a fichas que no tengan ninguna Habilidad
{
    public int speed;
    public int cooldown;
    public string label;
    
    public Ficha(int speed, int cooldown, string label)
    {
        this.speed = speed;
        this.cooldown = cooldown;
        this.label = label;
    }

    public abstract void Skill();

    public void MostrarInformacion()
    {
        Console.WriteLine($"Velocidad: {speed}, Tiempo de recarga: {cooldown}, Etiqueta: {label}");
    }
}

public class NormieChip : Ficha
{
    public NormieChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}
    public override void Skill(){}
}

public class CooldownChip : Ficha
{
    public CooldownChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}

    public override void Skill()
    {
        this.cooldown = cooldown - 2;
    }
}

public class SpeedChip : Ficha
{
    public SpeedChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}
    public override void Skill()
    {
        this.speed = speed*2;
    }
}

