using System.Collections.Generic;
using UnityEngine;

public class Casilla
{
    public Vector2Int Position { get; private set; }
    public bool EsTransitable { get; set; }
    public List<Ficha> FichasEnCasilla { get; private set; }
    public bool EsTrampa { get; set; }
    public bool EsInicio { get; set; }
    public bool EsSalida { get; set; }

    public Casilla(Vector2Int position, bool esTransitable, bool esTrampa = false, bool esInicio = false, bool esSalida = false)
    {
        Position = position;
        EsTransitable = esTransitable;
        FichasEnCasilla = new List<Ficha>();
        EsTrampa = esTrampa;
        EsInicio = esInicio;
        EsSalida = esSalida;
    }

    public void AgregarFicha(Ficha ficha)
    {
        if (!FichasEnCasilla.Contains(ficha))
        {
            FichasEnCasilla.Add(ficha);
        }
    }

    public void RemoverFicha(Ficha ficha)
    {
        if (FichasEnCasilla.Contains(ficha))
        {
            FichasEnCasilla.Remove(ficha);
        }
    }

    public void ActivarTrampa()
    {
        
    }

    public bool PuedeMoverse()
    {
        return EsTransitable && FichasEnCasilla.Count == 0;
    }
}
