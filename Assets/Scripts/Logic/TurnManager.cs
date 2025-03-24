using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public Player CurrentPlayer {get; private set;}
    public Ficha FichaSeleccionada {get; private set;}
    public FichaController FichaSeleccionadaCont {get; private set;}
    public bool EsperandoSeleccion {get; private set;}
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
        foreach (Player p in GameContext.Instance.players)
        {
            foreach (Ficha ficha in p.fichas)
            {
                if (ficha.visual != null && ficha.visual.button != null)
                {
                    ficha.visual.button.interactable = false;
                }
            }
        }        
        CurrentPlayer = player;
        EsperandoSeleccion = true;
        turnText.text = $"Es turno de {player.name}. Por favor selecciona una ficha";
        HabilitarSeleccionFichas(true);
        foreach (Ficha ficha in player.fichas)
        {
            ficha.ResetTurn();
            if (ficha.visual != null && ficha.visual.button != null)
            {
                ficha.visual.button.interactable = true;
            }
        }
        ActualizarUI();
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
            if (FichaSeleccionada != null)
            {
                FichaSeleccionada.visual.OnDeselected();
                FichaSeleccionadaCont = null;
            }

            FichaSeleccionada = ficha;
            FichaSeleccionadaCont = ficha.visual.GetComponent<FichaController>();
            EsperandoSeleccion = false;
            Debug.Log($"{CurrentPlayer.name} seleccionó: {ficha.label}");
            ficha.visual.OnSelected();
            HabilitarSeleccionFichas(false);
            ActualizarUI();
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

    public void ActualizarUI()
    {
        if (FichaSeleccionada != null && CurrentPlayer != null)
        {
            turnText.text = $"Turno: {CurrentPlayer.name} | Movimientos: {FichaSeleccionada.speed - FichaSeleccionada.currentSteps}/{FichaSeleccionada.speed}";
        }
        else
        {
            turnText.text = $"Turno: {CurrentPlayer?.name ?? "Ninguno"}";
        }
    }

    public void FinalizarAccion()
    {
        if(FichaSeleccionada != null)
        {
            FichaSeleccionada.ResetTurn();
            FichaSeleccionada.visual.OnDeselected();
            FichaSeleccionada = null;
        }
        EsperandoSeleccion = true;
        EndTurn();    
    }
}