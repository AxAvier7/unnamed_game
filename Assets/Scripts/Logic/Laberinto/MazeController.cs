using UnityEngine;

public class MazeController : MonoBehaviour
{
    public GameContext gameContext;
    public GameObject fichaPrefab;
    public Transform mazeGrid;

    void Start()
    {
        gameContext = GameContext.Instance;
    }


    public void PlaceFichasInMaze()
    {
        Transform casillaEntrada = GetMazeEntrance();
        foreach (var player in gameContext.players)
        {
            foreach (var ficha in player.fichas)
            {
                GameObject fichaObj = Instantiate(fichaPrefab, casillaEntrada);
                fichaObj.transform.localPosition = Vector3.zero;
            }
        }
    }

    Transform GetMazeEntrance()
    {
        foreach(Transform child in mazeGrid)
        { 
            Casilla casilla = child.GetComponent<Casilla>();
            if(casilla != null && casilla.EsInicio)
                return child;
        }
        return null;    
    }
}