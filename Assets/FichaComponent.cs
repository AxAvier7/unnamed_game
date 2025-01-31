using UnityEngine;

public class FichaComponent : MonoBehaviour
{
    public Ficha FichaData { get; private set; }
    public Casilla CurrentCasilla;

    public void Initialize(Ficha ficha, Casilla casilla)
    {
        FichaData = ficha;
        CurrentCasilla = casilla;

        if (FichaData == null)
        {
            Debug.LogError($"FichaData es null en {gameObject.name}.");
        }
        else
        {
            Debug.Log($"Ficha {FichaData.label} inicializada en {gameObject.name}.");
        }
    }
}
