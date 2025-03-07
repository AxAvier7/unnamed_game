using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FichaPrefab
{
    public TipoFicha tipo;
    public GameObject prefab;
}

public class MazeController : MonoBehaviour
{
    public FichaPrefab[] fichasPrefabs;
    public static MazeController Instance { get; private set; }
    public GameContext gameContext;
    public Transform mazeGrid;
    public List<Color> playerColors = new List<Color> {Color.red, Color.blue, Color.green, Color.magenta};

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else    Destroy(gameObject);
    }
    
    void Start()
    {
        gameContext = GameContext.Instance;
    }

    public void PlaceFichasInMaze()
    {
        Transform casillaEntrada = GetMazeEntrance();
        Casilla casillaComponent = casillaEntrada.GetComponent<Casilla>();
        foreach(var player in gameContext.players)
        {                
            int playerIndex = GameContext.Instance.players.IndexOf(player);
            Color playerColor = playerColors[playerIndex];

            foreach(var ficha in player.fichas)
            {
                GameObject prefab = ObtenerPrefabPorTipo(ficha.Tipo);
                GameObject fichaObj = Instantiate(prefab, casillaEntrada);
                
                fichaObj.GetComponent<Image>().color = playerColor;
                fichaObj.transform.localPosition = new Vector3(
                Random.Range(-30f, 30f),
                Random.Range(-30f, 30f),
                0);

                FichaComponent fichaComponent = fichaObj.GetComponent<FichaComponent>();
                fichaComponent.Initialize(ficha, casillaComponent);

                FichaController fichaController = fichaObj.GetComponent<FichaController>();
                fichaController.Initialize(fichaComponent);
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

    public Casilla GetCasilla(int x, int y)
    {
        foreach(Transform child in mazeGrid)
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

        foreach(int[] dir in direcciones)
        {
            Casilla vecina = GetCasilla(casilla.Coordenadas.x + dir[0], casilla.Coordenadas.y + dir[1]);
            if (vecina != null)
                vecinas.Add(vecina);
        }
        return vecinas;
    }

    public Casilla GetCasillaAleatoriaValida()
    {
        List<Casilla> casillasValidas = new List<Casilla>();
        foreach(Transform child in mazeGrid)
        {
            Casilla casilla = child.GetComponent<Casilla>();
            if(casilla != null && casilla.EsTransitable && !casilla.EsInicio)
                casillasValidas.Add(casilla);
        }
        if(casillasValidas.Count > 0)
        {
            return casillasValidas[Random.Range(0, casillasValidas.Count)];
        }
        return null;
    }

    private GameObject ObtenerPrefabPorTipo(TipoFicha tipo)
    {
        foreach(var p in fichasPrefabs)
        {
            if(p.tipo == tipo)
                return p.prefab;
        }
        return null;
    }
}