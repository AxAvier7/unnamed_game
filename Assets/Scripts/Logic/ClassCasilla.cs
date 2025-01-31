using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
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

    // private void Update()
    // {
    //     AjustarCollider();
    // }

    public Casilla(Vector2Int position, bool esTransitable, bool esTrampa = false, bool esInicio = false, bool esSalida = false)
    {
        Position = position;
        EsTransitable = esTransitable;
        FichasDelJugador = new List<Ficha>();
        EsTrampa = esTrampa;
        EsInicio = esInicio;
        EsSalida = esSalida;
    }


    // private void AjustarCollider()
    // {
    //     BoxCollider2D collider = GetComponent<BoxCollider2D>();
    //     RectTransform transform = GetComponent<RectTransform>();

    //     if (collider != null && transform != null)
    //     {
    //         collider.size = transform.rect.size;
    //     }
    // }
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
