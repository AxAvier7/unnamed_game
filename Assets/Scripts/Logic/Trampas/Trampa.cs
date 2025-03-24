using UnityEngine;
using UnityEngine.UI;

public class CasillaTrampa : Casilla
{
    public int Modificador { get; private set; }
    public bool Activada { get; private set; }
    public TipoEfectoTrampa efectoTrampa;
    public enum TipoEfectoTrampa
    {
        MultiplicarCooldown,
        DividirVelocidad,
        RegresarEntrada,
        Teletransportar
    }

    public CasillaTrampa(Vector2Int position, bool esTransitable, TipoEfectoTrampa efecto, int modificador = 2)
        : base(position, esTransitable, esTrampa: true)
    {
        efectoTrampa = efecto;
        Modificador = modificador;
        Activada = false;
    }

    public void ActivarTrampa(FichaComponent ficha)
    {
        if (!Activada && ficha.FichaData.Owner.turnosRestantesInmunidad == 0)
        {
            Activada = true;
            FichaController fichaController = ficha.GetComponent<FichaController>();
            Debug.Log($"¡Trampa activada en {Coordenadas}! Efecto: {efectoTrampa}");
            string message = "";

            switch (efectoTrampa)
            {
                case TipoEfectoTrampa.MultiplicarCooldown:
                    ficha.FichaData.cooldown *= 2;
                    message = $"{ficha.FichaData.label} - Cooldown x{Modificador}!";
                    break;

                case TipoEfectoTrampa.DividirVelocidad:
                    ficha.FichaData.speed = Mathf.Max(1, ficha.FichaData.speed / 2);
                    message = $"{ficha.FichaData.label} - Velocidad reducida!";
                    break;

                case TipoEfectoTrampa.RegresarEntrada:
                    Transform entrada = MazeController.Instance.GetMazeEntrance();
                    if (entrada != null)
                    {
                        Casilla casillaEntrada = entrada.GetComponent<Casilla>();
                        fichaController.ActualizarPosicion(casillaEntrada);
                        ficha.transform.SetParent(entrada);
                        ficha.transform.localPosition = Vector3.zero;
                        ficha.CurrentCasilla = entrada.GetComponent<Casilla>();
                        ficha.FichaData.currentPosition = casillaEntrada.Coordenadas;
                        message = $"{ficha.FichaData.label} - ¡Regresado a la entrada!";
                    }
                    break;

                case TipoEfectoTrampa.Teletransportar:
                    Casilla casillaAleatoria = MazeController.Instance.GetCasillaAleatoriaValida();
                    if (casillaAleatoria != null)
                    {
                        fichaController.ActualizarPosicion(casillaAleatoria);
                        ficha.transform.SetParent(casillaAleatoria.transform);
                        ficha.transform.localPosition = Vector3.zero;
                        ficha.CurrentCasilla = casillaAleatoria;
                        ficha.FichaData.currentPosition = casillaAleatoria.Coordenadas;
                        message = $"{ficha.FichaData.label} - ¡Teletransportado!";
                    }
                    if(casillaAleatoria.EsSalida)
                    {
                        string nombreJugador = "Jugador Desconocido";
                        foreach (var player in GameContext.Instance.players)
                        {
                            nombreJugador = player.name;
                            break;
                        }
                        Victory.Instance.ShowVictory(nombreJugador);                    
                    }
                    break;
            }
            TrapManager.Instance.ShowTrapEffect(message);
            EsTrampa = false;
            GetComponent<Image>().color = Color.white;
            TrapManager.Instance.EliminarTrampa(Coordenadas);
        }
    }
    public void InicializarCasillaTrampa(Casilla original)
    {
        Activada = false;
        Coordenadas = original.Coordenadas;
        EsTransitable = original.EsTransitable;
        EsInicio = original.EsInicio;
        EsSalida = original.EsSalida;
        efectoTrampa = TipoEfectoTrampa.Teletransportar;
        GetComponent<Image>().color = Color.magenta;
        TrapManager.Instance.RegistrarTrampa(Coordenadas, this);
    }
}