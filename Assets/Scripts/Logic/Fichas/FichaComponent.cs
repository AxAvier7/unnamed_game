using UnityEngine;
using UnityEngine.UI;

public class FichaComponent : MonoBehaviour
{
    public Ficha FichaData { get; private set;}
    public Button button;
    public Casilla CurrentCasilla;
    public Color colorOriginal;
    public FichaController controller;

    void Start()
    {
        button = GetComponent<Button>();
        colorOriginal = GetComponent<Image>().color;
        button.onClick.AddListener(OnFichaClick);
    }

    public void Initialize(Ficha ficha, Casilla casilla)
    {
        FichaData = ficha;
        CurrentCasilla = casilla;
        FichaData.currentPosition = casilla.Coordenadas;
        FichaData.Controller = GetComponent<FichaController>();
        FichaData.visual = this;
        controller = GetComponent<FichaController>();

    }

    private void OnFichaClick()
    {
        if(GameContext.Instance.gameStarted && TurnManager.Instance.EsperandoSeleccion)
        {        
            if(FichaData.Owner == TurnManager.Instance.CurrentPlayer)
            {
                TurnManager.Instance.SeleccionarFicha(FichaData);
                GetComponent<Image>().color = Color.yellow;
            }
        }    
    }

    public void OnSelected()
    {
        GetComponent<Image>().color = Color.yellow;
    }

    public void OnDeselected()
    {
        GetComponent<Image>().color = colorOriginal;
    }
}