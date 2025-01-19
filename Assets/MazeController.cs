using UnityEngine;

public class MazeController : MonoBehaviour
{
    public GameContext gameContext;
    public GameObject fichaPrefab;
    public Transform mazeGrid;
    private Ficha ficha;
    private GameObject fichaInstance;

    void Start()
    {
        gameContext = GameContext.Instance;
        PlaceFichasInMaze();
    }

    void PlaceFichasInMaze()
    {
        foreach (var player in gameContext.players)
        {
            foreach (var ficha in player.fichas)
            {
                Vector3 position = new Vector3(0, 0, 0);
                fichaInstance = Instantiate(fichaPrefab, position, Quaternion.identity);
                fichaInstance.name = ficha.label;
                fichaInstance.transform.position = position;
            }
        }
    }

    void Update()
    {
        if (ficha == null) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TryMove(Vector2Int.up);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TryMove(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TryMove(Vector2Int.right);
        if (Input.GetKeyDown(KeyCode.Return))   ConfirmMove();
    }

    void TryMove(Vector2Int direction)
    {
        if (!ficha.CanMove()) return;

        Vector2Int targetPosition = ficha.currentPosition + direction;

        if (IsPositionValid(targetPosition))
        {
            ficha.currentPosition = targetPosition;
            ficha.currentSteps++;
            fichaInstance.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
        }
        else    Debug.Log("Movimiento inválido: fuera de límites o en casilla no transitable.");
    }

    bool IsPositionValid(Vector2Int position)
    {
        if (position.x < 0 || position.y < 0 ||
            position.x >= gameContext.maze.GetLength(1) || position.y >= gameContext.maze.GetLength(0))
        {
            return false;
        }
        return gameContext.maze[position.y, position.x] == 1;
    }

    void ConfirmMove()
    {
        ficha.ConfirmMove();
        Debug.Log($"Movimiento confirmado en la posición {ficha.currentPosition}");
    }
}
