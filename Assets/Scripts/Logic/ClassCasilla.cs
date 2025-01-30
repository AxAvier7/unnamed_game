using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    public bool EsTransitable { get; set; }
    private Player CurrentPlayer {get; set;}
    public List<Ficha> FichasDelJugador { get; private set; }
    public bool EsTrampa { get; set; }
    public bool EsInicio { get; set; }
    public bool EsSalida { get; set; }
    public Vector2Int Coordenadas;

    public Casilla(Vector2Int position, bool esTransitable, bool esTrampa = false, bool esInicio = false, bool esSalida = false)
    {
        Position = position;
        EsTransitable = esTransitable;
        FichasDelJugador = new List<Ficha>();
        EsTrampa = esTrampa;
        EsInicio = esInicio;
        EsSalida = esSalida;
    }

    public void ActivarTrampa()
    {
        if(EsTrampa)
        {
            Debug.Log($"¡Trampa activada en la posición {Position}!");
        }
    }

    public bool PuedeMoverse()
    {
        return EsTransitable && CurrentPlayer == null;
    }
}
