using UnityEngine;

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
        if (!Activada)
        {
            Activada = true;
            Debug.Log($"¡Trampa activada en {Coordenadas}! Efecto: {efectoTrampa}");
            string message = "";

            switch (efectoTrampa)
            {
                case TipoEfectoTrampa.MultiplicarCooldown:
                    ficha.FichaData.cooldown *= Modificador;
                    message = $"{ficha.FichaData.label} - Cooldown x{Modificador}!";
                    break;

                case TipoEfectoTrampa.DividirVelocidad:
                    ficha.FichaData.speed = Mathf.Max(1, ficha.FichaData.speed / Modificador);
                    message = $"{ficha.FichaData.label} - Velocidad reducida!";
                    break;

                case TipoEfectoTrampa.RegresarEntrada:
                    Transform entrada = MazeController.Instance.GetMazeEntrance();
                    if (entrada != null)
                    {
                        Casilla casillaEntrada = entrada.GetComponent<Casilla>();
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
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}