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
        Debug.Log($"Velocidad de la ficha {label} de {Owner.name} duplicada");
    }
}

public class ThunderChip : Ficha
{
    public ThunderChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Thunder;}
    public override void Skill()
    {
        foreach(Player player in GameContext.Instance.players)
            {
                if(player != this.Owner)
                    player.isParalized = true;
            }

            TurnManager.Instance.turnosExtraRestantes = 1;
            speed *= 2;
            cooldown = 4;
            Debug.Log($"{Owner.name} ha paralizado a los rivales y ahora es más rapido");
    }
}

public class ShieldChip : Ficha
{
    public ShieldChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Shield;}
    public override void Skill()
    {
        Owner.turnosRestantesInmunidad = 3;
        cooldown = 5;
        Debug.Log($"{Owner.name} se ha vuelto inmune por 3 turnos.");    
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
        Debug.Log($"{Owner.name} se ha teletransportado.");
    }
}

public class TrapChip : Ficha
{
    public TrapChip(int speed, int cooldown, string label) : base(speed, cooldown, label)
    {Tipo = TipoFicha.Trap;}
    public override void Skill()
    {
        Vector2Int posicionActual = currentPosition;
        MazeController.Instance.PlaceTrap(posicionActual);
        cooldown = 4;
        Debug.Log($"{Owner.name} coloco una trampa en ({posicionActual.x},{posicionActual.y})");
    }
}

public enum TipoFicha
{
    Normie,
    Cooldown,
    Speed,
    Teleport,
    Thunder,
    Shield,
    Trap
}