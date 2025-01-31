using System.Collections.Generic;
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

                
                FichaComponent fichaComponent = fichaObj.GetComponent<FichaComponent>();
                Casilla casillaComponent = casillaEntrada.GetComponent<Casilla>();

                if (fichaComponent != null && casillaComponent != null)
                {
                    fichaComponent.Initialize(ficha, casillaComponent);
                    Debug.Log($"Ficha {ficha.label} inicializada en la casilla ({casillaComponent.Coordenadas.x}, {casillaComponent.Coordenadas.y})");
                }
                else
                {
                    Debug.LogError("FichaComponent o Casilla no encontrados en la ficha instanciada.");
                }
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
}