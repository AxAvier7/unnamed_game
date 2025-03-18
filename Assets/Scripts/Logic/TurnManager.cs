using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public Player CurrentPlayer {get; private set;}
    public Ficha FichaSeleccionada {get; private set;}
    public FichaController FichaSeleccionadaCont {get; private set;}
    public bool EsperandoSeleccion {get; private set;}
    public GameObject CoordsPanel;
    public Text turnText;
    public int turnosExtraRestantes = 0;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartTurn(Player player)
    {
        CurrentPlayer = player;
        EsperandoSeleccion = true;
        turnText.text = $"Es turno de {player.name}. Por favor selecciona una ficha";
        HabilitarSeleccionFichas(true);
    }

    public void EndTurn()
    {
        if(turnosExtraRestantes > 0)
        {
            turnosExtraRestantes--;
            foreach (Player jugador in GameContext.Instance.players)
            {
                jugador.isParalized = false;
            }
            StartTurn(CurrentPlayer);
            Debug.Log($"Turno extra restante: {turnosExtraRestantes}");
        }
        else
        {
            int currentPlayerIndex = GameContext.Instance.players.IndexOf(CurrentPlayer);
            if(currentPlayerIndex == GameContext.Instance.players.Count - 1)
            {
                GameContext.Instance.currentTurns++;
            }
            if (CurrentPlayer != null && CurrentPlayer.turnosRestantesInmunidad > 0)
            {
                CurrentPlayer.turnosRestantesInmunidad--;
                Debug.Log($"Inmunidad restante: {CurrentPlayer.turnosRestantesInmunidad} turnos");
            }
            foreach (Player player in GameContext.Instance.players)
            {
                foreach (Ficha ficha in player.fichas)
                {
                    ficha.cooldown = Mathf.Max(0, ficha.cooldown - 1);
                }
            }
            int nextIndex = (currentPlayerIndex + 1) % GameContext.Instance.players.Count;
            Player nextPlayer = GameContext.Instance.players[nextIndex];
            while(nextPlayer.isParalized)
            {
                nextPlayer.isParalized = false;
                nextIndex = (nextIndex + 1) % GameContext.Instance.players.Count;
                nextPlayer = GameContext.Instance.players[nextIndex];
            }
            StartTurn(nextPlayer);
            turnText.text = $"Turno de: {CurrentPlayer.name} | Ronda: {GameContext.Instance.currentTurns}";    
        }
    }

    public void SeleccionarFicha(Ficha ficha)
    {
        if(EsperandoSeleccion && ficha.Owner == CurrentPlayer)
        {
            if(FichaSeleccionada != null && FichaSeleccionada.visual != null)
            {
                FichaSeleccionadaCont = ficha.visual.GetComponent<FichaController>();
                FichaSeleccionada.visual.OnDeselected();
            }
            FichaSeleccionada = ficha;
            EsperandoSeleccion = false;
            Debug.Log ($"{CurrentPlayer.name} ha seleccionado {ficha.label}");
            ficha.visual.OnSelected();
            HabilitarSeleccionFichas(false);
            HabilitarAccionesFicha(true);
        }
    }

    private void HabilitarSeleccionFichas(bool habilitar)
    {
        foreach(Ficha ficha in CurrentPlayer.fichas)
        {
            if(ficha.visual != null && ficha.visual.button != null)
            {
                ficha.visual.button.interactable = habilitar;
            }
        }    
    }

    private void HabilitarAccionesFicha(bool habilitar)
    {
        CoordsPanel.SetActive(habilitar);
    }
}