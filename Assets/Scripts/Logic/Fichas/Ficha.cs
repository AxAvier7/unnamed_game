using System;
using UnityEngine;

public abstract class Ficha //esta es la clase que define a las fichas basicas. Con esta creare a fichas que no tengan ninguna habilidad
{
    public int speed;
    public int cooldown;
    public string label;
    public Vector2Int currentPosition;
    public int currentSteps;
    public bool moveConfirmation;
    
    public Ficha(int speed, int cooldown, string label)
    {
        this.speed = speed;
        this.cooldown = cooldown;
        this.label = label;
        currentSteps = 0;
        moveConfirmation = false;
    }

    public abstract void Skill();

    public void ResetTurn()
    {
        currentSteps = 0;
        moveConfirmation = false;
    }

    public void MostrarInformacion()
    {
        Debug.Log($"Velocidad: {speed}, Tiempo de recarga: {cooldown}, Etiqueta: {label}");
    }

    public bool CanMove(){  return currentSteps < speed && !moveConfirmation;   }
    public void ConfirmMove(){  moveConfirmation = true;    }
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
        this.cooldown = Math.Max(0, cooldown - 2);
    }
}

public class SpeedChip : Ficha
{
    public SpeedChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}
    public override void Skill()
    {
        this.speed *= 2;
    }
}

public class InvisibilityChip : Ficha
{
    public InvisibilityChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}

    public override void Skill()
    {
        Debug.Log("El jugador se vuelve invisible temporalmente.");
    }
}

public class ShieldChip : Ficha
{
    public ShieldChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}

    public override void Skill()
    {
        Debug.Log("Escudo activado, protegiendo al jugador.");
    }
}

public class TeleportChip : Ficha
{
    public TeleportChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}

    public override void Skill()
    {
        Debug.Log("El jugador ha sido teletransportado.");
    }
}

public class TrapChip : Ficha
{
    public TrapChip(int speed, int cooldown, string label) : base(speed, cooldown, label){}

    public override void Skill()
    {
        Debug.Log("Trampa colocada, ralentizando a los enemigos.");
    }
}