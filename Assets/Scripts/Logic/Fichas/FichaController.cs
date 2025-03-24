using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FichaController : MonoBehaviour
{
    private FichaComponent fichaComponent;
    public Ficha fichaData;
    private Casilla casillaActual;
    private MazeController mazeController;

    void Update()
    {
        if(TurnManager.Instance.FichaSeleccionada == fichaData)
        {
            if(!TurnManager.Instance.EsperandoSeleccion) 
                MovimientoWASD();
                
            if(Input.GetKeyDown(KeyCode.Space)) 
                ConfirmarMovimiento();
        }    
    }

    private void MovimientoWASD()
    {
        if(GameContext.Instance.gameStarted && fichaData.currentSteps >= fichaData.speed) return;
        Vector2Int direccion = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) direccion = Vector2Int.left; //Subir
        else if (Input.GetKeyDown(KeyCode.S)) direccion = Vector2Int.right; //Bajar
        else if (Input.GetKeyDown(KeyCode.A)) direccion = Vector2Int.down; //Izquierda
        else if (Input.GetKeyDown(KeyCode.D)) direccion = Vector2Int.up; //Derecha

        if(direccion != Vector2Int.zero)
        {
            IntentarMover(direccion);
        }
    }

    private void IntentarMover(Vector2Int direccion)
    {
        if(fichaData.CanMove())
        {
            Vector2Int targetPos = fichaData.currentPosition + direccion;
            Casilla targetCasilla = MazeController.Instance.GetCasilla(targetPos.x, targetPos.y);

            if(targetCasilla != null && (targetCasilla.EsTransitable || (targetCasilla is CasillaTrampa trampa && !trampa.Activada)))
            {
                if(fichaData.historialMovimientos.Count > 0 && targetPos == fichaData.historialMovimientos.Peek())
                {
                    fichaData.currentPosition = targetPos;
                    fichaData.historialMovimientos.Pop();
                    MoverFicha(targetCasilla);
                }
                else
                {
                    if(fichaData.currentSteps < fichaData.speed)
                    {
                        fichaData.historialMovimientos.Push(fichaData.currentPosition);
                        fichaData.currentSteps++;
                        fichaData.currentPosition = targetPos;
                        MoverFicha(targetCasilla);
                    }
                }
                ActualizarFeedbackUI();
            }            
            else
            {
                StartCoroutine(MostrarMovimientoInvalido(targetCasilla));
            }
        }
    }

    private void MoverFicha(Casilla destino)
    {
        if (destino == null || !destino.EsTransitable)
        {
            TurnManager.Instance.turnText.text = "Casilla destino inválida";
            return;
        }

        transform.SetParent(destino.transform);
        transform.localPosition = Vector3.zero;
        casillaActual = destino;
        fichaData.currentPosition = destino.Coordenadas;
        if (destino.EsSalida)
        {
            string nombreJugador = fichaData.Owner.name;
            Victory.Instance.ShowVictory(nombreJugador);
        }
    }

    private void ActualizarFeedbackUI()
    {
        TurnManager.Instance.turnText.text = $"Movimientos restantes: {fichaData.speed - fichaData.currentSteps}/{fichaData.speed}";
    }

    public void ConfirmarMovimiento()
    {
        fichaData.ConfirmMove();
        fichaData.ResetTurn();
        TurnManager.Instance.FinalizarAccion();
        TurnManager.Instance.ActualizarUI();

        if (casillaActual != null && casillaActual is CasillaTrampa)
        {
            CasillaTrampa trampa = (CasillaTrampa)casillaActual;
            trampa.ActivarTrampa(fichaComponent);
        }
    }

    public void Initialize(FichaComponent component)
    {
        fichaComponent = component;
        fichaData = component.FichaData;
        casillaActual = transform.parent.GetComponent<Casilla>();
        mazeController = MazeController.Instance;
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

    public void MoverFichaACasilla(Casilla nuevaCasilla) 
    {
        if(!TurnManager.Instance.EsperandoSeleccion && 
        TurnManager.Instance.FichaSeleccionada == this.fichaData)
        {
            transform.SetParent(nuevaCasilla.transform);
            transform.localPosition = Vector3.zero;
            casillaActual = nuevaCasilla;
            fichaData.currentPosition = nuevaCasilla.Coordenadas;
            fichaData.currentSteps = 0;
            Debug.Log($"{fichaData.label} teletransportada a ({nuevaCasilla.Coordenadas.x}, {nuevaCasilla.Coordenadas.y})");
        }
    }

    public void UsarHabilidad()
    {
        if(TurnManager.Instance.FichaSeleccionada == null)
        {
            Debug.LogError("No hay ficha seleccionada.");
            return;
        }

        Ficha fichaActual = TurnManager.Instance.FichaSeleccionada;
        if(fichaActual.CanUseSkill)
        {
            fichaActual.Skill();
            TurnManager.Instance.EndTurn();
        }  
    }
    private IEnumerator MostrarMovimientoInvalido(Casilla casilla)
    {
        if (casilla != null)
        {
            Image img = casilla.GetComponent<Image>();
            Color original = img.color;
            img.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            img.color = original;
        }
    }
}