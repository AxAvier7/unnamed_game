using UnityEngine;

public abstract class Ficha //esta es la clase que define a las fichas basicas. Con esta creare a fichas que no tengan ninguna habilidad
{
    public FichaController Controller {get; set;} 
    public int speed;
    public int cooldown;
    public string label;
    public Vector2Int currentPosition;
    public int currentSteps;
    public bool moveConfirmation;
    public bool isChosen;
    public Player Owner;
    public FichaComponent visual;
    public bool CanUseSkill => cooldown <= 0;
    public TipoFicha Tipo { get; set; }
    
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

    public bool CanMove(){  return currentSteps < speed && !moveConfirmation;   }
    public void ConfirmMove(){  moveConfirmation = true;    }
    public void ReducirCooldownDeFichas(int cantidad)
    {
        if(Owner == null) return;
        foreach(Ficha ficha in Owner.fichas)
        {
            if(ficha!=this)     ficha.cooldown = Mathf.Max(0, ficha.cooldown - cantidad);
        }
    }
}

public class NormieChip : Ficha
{
    public NormieChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Normie;}
    public override void Skill(){}
}

public class CooldownChip : Ficha
{
    public CooldownChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Cooldown;}
    public override void Skill()
    {
        ReducirCooldownDeFichas(2);
    }
}

public class SpeedChip : Ficha
{
    public SpeedChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Speed;}
    public override void Skill()
    {
        speed *= 2;
        cooldown = 3;
        Debug.Log("Velocidad duplicada");
    }
}

public class InvisibilityChip : Ficha
{
    public InvisibilityChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Invisibility;}
    public override void Skill()
    {
        GameContext.Instance.CurrentPlayer.isInvisible = true;
        cooldown = 5;
        Debug.Log("El jugador se ha vuelto invisible.");
    }
}

public class ShieldChip : Ficha
{
    public ShieldChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Shield;}
    public override void Skill()
    {
        cooldown = 5;
        Debug.Log("Escudo activado.");
    }
}

public class TeleportChip : Ficha
{
    public TeleportChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Teleport;}
    public override void Skill()
    {
        Casilla destino = MazeController.Instance.GetCasillaAleatoriaValida();
        Controller.MoverFichaACasilla(destino);
        cooldown = 5;
        Debug.Log("El jugador se ha teletransportado.");
    }
}

public class TrapChip : Ficha
{
    public TrapChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Trap;}
    public override void Skill()
    {
        Debug.Log("Trampa colocada.");
    }
}

public enum TipoFicha
{
    Normie,
    Cooldown,
    Speed,
    Teleport,
    Invisibility,
    Shield,
    Trap
}