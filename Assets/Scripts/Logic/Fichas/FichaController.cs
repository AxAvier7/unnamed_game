using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FichaController : MonoBehaviour
{
    private FichaComponent fichaComponent;
    public InputField inputX;
    public InputField inputY;
    public Button moveButton;
    public Ficha fichaData;
    private Casilla casillaActual;
    private MazeController mazeController;

    public void Initialize(FichaComponent component)
    {
        fichaComponent = component;
        fichaData = component.FichaData;
        casillaActual = transform.parent.GetComponent<Casilla>();
        mazeController = MazeController.Instance;
        moveButton.onClick.AddListener(TryMove);
    }

    private void TryMove()
    {
        if(!int.TryParse(inputX.text, out int destinoX) || !int.TryParse(inputY.text, out int destinoY))
        {
            Debug.LogError("Las coordenadas ingresadas no son válidas.");
            return;
        }

        Casilla casillaDestino = mazeController.GetCasilla(destinoX, destinoY);
        if(casillaDestino == null)
        {
            Debug.LogError($"No existe una casilla en las coordenadas ({destinoX}, {destinoY}).");
            return;
        }

        if(!casillaDestino.EsTransitable)
        {
            Debug.LogError($"La casilla ({destinoX}, {destinoY}) no es transitable.");
            return;
        }

        if(PuedeLlegarA(casillaActual, casillaDestino, fichaData.speed))
        {
            MoverFicha(casillaDestino);
        }
        else
        {
            Debug.LogError($"La ficha {fichaData.label} no puede llegar a ({destinoX}, {destinoY}) con su velocidad de {fichaData.speed}.");
            Debug.Log($"La ficha esta en {fichaData.currentPosition.x}, {fichaData.currentPosition.y}");
        }
    }

    private void MoverFicha(Casilla nuevaCasilla)
    {
        fichaData.currentSteps += CalcularDistancia(casillaActual, nuevaCasilla);
        transform.SetParent(nuevaCasilla.transform);
        transform.localPosition = Vector3.zero;
        casillaActual = nuevaCasilla;
        Vector2Int coordenadasFicha = nuevaCasilla.Coordenadas;
        fichaData.currentPosition = coordenadasFicha;
        if(TrapManager.Instance.HayTrampaEn(coordenadasFicha, out CasillaTrampa trampa))
        {
            FichaComponent fichaComponent = GetComponent<FichaComponent>();
            if (fichaComponent != null)
            {
                trampa.ActivarTrampa(fichaComponent);
                TrapManager.Instance.ShowTrapEffect($"¡Trampa: {trampa.efectoTrampa}!");
            }
        }
        if(nuevaCasilla.EsSalida)
        {
            string nombreJugador = "Jugador Desconocido";
            foreach (var player in GameContext.Instance.players)
            {
                if (player.fichas.Contains(fichaData))
                {
                    nombreJugador = player.name;
                    break;
                }
            }
            Victory.Instance.ShowVictory(nombreJugador);
        }
        Debug.Log($"Ficha {fichaData.label} movida a casilla ({nuevaCasilla.Coordenadas.x}, {nuevaCasilla.Coordenadas.y}).");
    }

    private bool PuedeLlegarA(Casilla origen, Casilla destino, int maxPasos)
    {
        List<Casilla> visitadas = new List<Casilla>();
        return CheckDistance(origen, destino, maxPasos, visitadas);
    }

    private bool CheckDistance(Casilla actual, Casilla destino, int pasosRestantes, List<Casilla> visitadas)
    {
        if(actual == destino) return true;
        if(pasosRestantes <= 0) return false;
        visitadas.Add(actual);

        foreach(Casilla vecino in mazeController.GetCasillasVecinas(actual))
        {
            if(!visitadas.Contains(vecino) && vecino.EsTransitable)
            {
                if(CheckDistance(vecino, destino, pasosRestantes - 1, visitadas))
                    return true;
            }
        }
        return false;
    }

    private int CalcularDistancia(Casilla origen, Casilla destino)
    {
        return Mathf.Abs(origen.Coordenadas.x - destino.Coordenadas.x) + Mathf.Abs(origen.Coordenadas.y - destino.Coordenadas.y);
    }

    public void ActualizarPosicion(Casilla nuevaCasilla)
    {
        transform.SetParent(nuevaCasilla.transform);
        transform.localPosition = Vector3.zero;
        casillaActual = nuevaCasilla;
        fichaData.currentPosition = nuevaCasilla.Coordenadas;
        fichaData.currentSteps = 0;
        fichaData.moveConfirmation = false;
    }
}