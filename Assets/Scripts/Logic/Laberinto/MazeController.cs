using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    public static MazeController Instance { get; private set; }
    public GameContext gameContext;
    public GameObject fichaPrefab;
    public Transform mazeGrid;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        gameContext = GameContext.Instance;
    }

    public void PlaceFichasInMaze()
    {
        Transform casillaEntrada = GetMazeEntrance();
        Casilla casillaComponent = casillaEntrada.GetComponent<Casilla>();
        foreach (var player in gameContext.players)
        {
            foreach (var ficha in player.fichas)
            {
                GameObject fichaObj = Instantiate(fichaPrefab, casillaEntrada);
                fichaObj.transform.localPosition = Vector3.zero;
                FichaComponent fichaComponent = fichaObj.GetComponent<FichaComponent>();
                if (fichaComponent != null)
                {
                    fichaComponent.Initialize(ficha, casillaComponent);
                    FichaController fichaController = fichaObj.GetComponent<FichaController>();
                    if (fichaController != null)
                    {
                        fichaController.Initialize(fichaComponent);
                    }
                }
            }
        }
    }

    public Transform GetMazeEntrance()
    {
        foreach(Transform child in mazeGrid)
        { 
            Casilla casilla = child.GetComponent<Casilla>();
            if(casilla != null && casilla.EsInicio)
                return child;
        }
        return null;    
    }   
     
    Transform GetMazeExit()
    {
        foreach(Transform child in mazeGrid)
        { 
            Casilla casilla = child.GetComponent<Casilla>();
            if(casilla != null && casilla.EsSalida)
                return child;
        }
        return null;    
    }

    public Casilla GetCasilla(int x, int y)
    {
        foreach (Transform child in mazeGrid)
        {
            Casilla casilla = child.GetComponent<Casilla>();
            if (casilla != null && casilla.Coordenadas.x == x && casilla.Coordenadas.y == y)
            {
                return casilla;
            }
        }
        return null;
    }

    public List<Casilla> GetCasillasVecinas(Casilla casilla)
    {
        List<Casilla> vecinas = new List<Casilla>();
        int[][] direcciones = { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };

        foreach (int[] dir in direcciones)
        {
            Casilla vecina = GetCasilla(casilla.Coordenadas.x + dir[0], casilla.Coordenadas.y + dir[1]);
            if (vecina != null)
            {
                vecinas.Add(vecina);
            }
        }
        return vecinas;
    }

    public Casilla GetCasillaAleatoriaValida()
    {
        List<Casilla> casillasValidas = new List<Casilla>();
        foreach (Transform child in mazeGrid)
        {
            Casilla casilla = child.GetComponent<Casilla>();
            if (casilla != null && casilla.EsTransitable && !casilla.EsInicio)
            {
                casillasValidas.Add(casilla);
            }
        }
        if (casillasValidas.Count > 0)
        {
            return casillasValidas[Random.Range(0, casillasValidas.Count)];
        }
        return null;
    }
}