using UnityEngine;

public class CasillaTrampa : Casilla
{
    public string Nombre { get; private set; }
    public string Descripcion { get; private set; }
    public int Modificador { get; private set; }
    public bool Activada { get; private set; }

    public CasillaTrampa(Vector2Int position, bool esTransitable, string nombre, string descripcion, int modificador)
        : base(position, esTransitable, esTrampa: true)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Modificador = modificador;
        Activada = false;
    }

    public void ActivarTrampa()
    {
        if (!Activada)
        {
            Activada = true;
            Debug.Log($"¡Trampa {Nombre} activada en la posición {Position}! {Descripcion}");
        }
    }
}