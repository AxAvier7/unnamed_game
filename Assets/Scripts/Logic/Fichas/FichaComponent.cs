using UnityEngine;

public class FichaComponent : MonoBehaviour
{
    public Ficha FichaData { get; private set; }
    public Casilla CurrentCasilla;

    public void Initialize(Ficha ficha, Casilla casilla)
    {
        FichaData = ficha;
        CurrentCasilla = casilla;
    }
}
