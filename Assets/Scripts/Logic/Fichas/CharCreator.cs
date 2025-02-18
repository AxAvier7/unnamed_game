using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject CoordsPanel;
    public Transform selectedFichasContainer;
    public GameObject fichaPrefab;
    public List<Button> fichaButtons;
    public Text playerIndicatorText;
    public Button confirmButton;

    private int currentPlayerIndex = 0;
    private List<Player> players = new List<Player>();
    private List<Ficha> currentSelectedFichas = new List<Ficha>();

    public GameData gameData;
    public GameContext gameContext;
    public MazeController mazeController;
    private List<Color> playerColors = new List<Color> {Color.red, Color.blue, Color.green, Color.yellow};

    void Start()
    {
        gameData = GameData.Instance;
        gameContext = GameContext.Instance;
        if (gameContext == null)    return;
        if (gameContext.players == null)
        {
            gameContext.players = new List<Player>();
        }
        characterPanel.SetActive(true);
        UpdatePlayerIndicatorText();        
        confirmButton.onClick.AddListener(ConfirmSelection);
        foreach (Button button in fichaButtons)
        {
            button.onClick.AddListener(() => OnFichaButtonClick(button));
        }
    }

    void UpdatePlayerIndicatorText()
    {
        string playerName = gameData.PlayerNames[currentPlayerIndex];
        playerIndicatorText.text = $"{playerName}, selecciona tus fichas";
    }

    void OnFichaButtonClick(Button button)
    {
        if (currentSelectedFichas.Count >= gameData.Chips)
        {
            Debug.Log("Ya seleccionaste el máximo de fichas permitidas.");
            return;
        }

        string fichaType = button.name;
        Ficha ficha = CreateFichaByType(fichaType);
        currentSelectedFichas.Add(ficha);
        GameObject newFichaUI = Instantiate(fichaPrefab, selectedFichasContainer);
        Image fichaImage = newFichaUI.GetComponent<Image>();
        fichaImage.sprite = button.GetComponent<Image>().sprite;
        newFichaUI.GetComponent<Image>().color = playerColors[currentPlayerIndex];
    }

    Ficha CreateFichaByType(string type)
    {
        switch (type)
        {
            case "NormieChip":
                return new NormieChip(5, 2, "Normie");
            case "CooldownChip":
                return new CooldownChip(4, 5, "Cooldown");
            case "SpeedChip":
                return new SpeedChip(6, 3, "Speed");
            case "InvisibilityChip":
                return new InvisibilityChip(5, 6, "Invisibility");
            case "ShieldChip":
                return new ShieldChip(2, 5, "Shield");
            case "TeleportChip":
                return new TeleportChip(4, 7, "Teleport");
            case "TrapChip":
                return new TrapChip(4, 6, "Trap");
            default:
                return new NormieChip(5, 2, "Default");
        }
    }

    void ConfirmSelection()
    {
        if (currentSelectedFichas.Count < gameData.Chips)
        {
            Debug.Log("Selecciona la cantidad correcta de fichas antes de confirmar.");
            return;
        }

        Player player = new NewPlayer(gameData.PlayerNames[currentPlayerIndex]);
        player.fichas.AddRange(currentSelectedFichas);
        gameContext.players.Add(player);
        players.Add(player);

        currentSelectedFichas.Clear();
        foreach (Transform child in selectedFichasContainer)
        {
            Destroy(child.gameObject);
        }

        currentPlayerIndex++;

        if (currentPlayerIndex < gameData.Players)
        {
            playerIndicatorText.text = $"{gameData.PlayerNames[currentPlayerIndex]} selecciona tus fichas";
        }
        else
        {
            characterPanel.SetActive(false);
            StartGame();
        }
    }

    void StartGame()
    {
        if (CoordsPanel != null)    CoordsPanel.SetActive(true);
        Debug.Log("Todos los jugadores han seleccionado sus fichas. Comienza el juego.");
        foreach(var player in players)
        {
            Debug.Log($"Jugador: {player.name}");
            foreach(var ficha in player.fichas)
            {
                Debug.Log($"Ficha: {ficha.label}, Velocidad: {ficha.speed}, Recarga: {ficha.cooldown}");
            }
        }
        if (mazeController != null && gameContext != null)
        {
            MazeController.Instance.PlaceFichasInMaze();
        }
    }
}