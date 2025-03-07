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
    public GameObject TurnsAndSkillsCanvas;

    private int currentPlayerIndex = 0;
    private List<Player> players = new List<Player>();
    private List<Ficha> currentSelectedFichas = new List<Ficha>();

    public GameData gameData;
    public GameContext gameContext;
    public MazeController mazeController;
    public List<Color> playerColors = new List<Color> {Color.red, Color.blue, Color.green, Color.magenta};

    void Start()
    {
        gameData = GameData.Instance;
        gameContext = GameContext.Instance;
        if(gameContext == null)    return;
        if(gameContext.players == null)
        {
            gameContext.players = new List<Player>();
        }
        characterPanel.SetActive(true);
        UpdatePlayerIndicatorText();        
        confirmButton.onClick.AddListener(ConfirmSelection);
        foreach(Button button in fichaButtons)
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
        if(currentSelectedFichas.Count >= gameData.Chips)
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
        fichaImage.color = playerColors[currentPlayerIndex];
    }

    Ficha CreateFichaByType(string type)
    {
        Ficha nuevaFicha;
        switch(type)
        {
            case "NormieChip":
                nuevaFicha = new NormieChip(3, 2, "Normie");
                nuevaFicha.Tipo = TipoFicha.Normie;
                break;
            case "CooldownChip":
                nuevaFicha = new CooldownChip(4, 5, "Cooldown");
                nuevaFicha.Tipo = TipoFicha.Cooldown;
                break;
            case "SpeedChip":
                nuevaFicha = new SpeedChip(5, 3, "Speed");
                nuevaFicha.Tipo = TipoFicha.Speed;
                break;
            case "InvisibilityChip":
                nuevaFicha = new InvisibilityChip(5, 5, "Invisibility");
                nuevaFicha.Tipo = TipoFicha.Invisibility;
                break;
            case "ShieldChip":
                nuevaFicha = new ShieldChip(2, 5, "Shield");
                nuevaFicha.Tipo = TipoFicha.Shield;
                break;
            case "TeleportChip":
                nuevaFicha = new TeleportChip(4, 5, "Teleport");
                nuevaFicha.Tipo = TipoFicha.Teleport;
                break;
            case "TrapChip":
                nuevaFicha = new TrapChip(4, 6, "Trap");
                nuevaFicha.Tipo = TipoFicha.Trap;
                break;
            default:
                return new NormieChip(5, 2, "Default"){Tipo = TipoFicha.Normie};
        }
        return nuevaFicha;
    }


    void ConfirmSelection()
    {
        if(currentSelectedFichas.Count < gameData.Chips)
        {
            Debug.Log("Selecciona la cantidad correcta de fichas antes de confirmar.");
            return;
        }

        Player player = new NewPlayer(gameData.PlayerNames[currentPlayerIndex]);
        player.fichas.AddRange(currentSelectedFichas);
        foreach(var ficha in currentSelectedFichas)
        {
            ficha.Owner = player;
        }
        gameContext.players.Add(player);
        players.Add(player);

        currentSelectedFichas.Clear();
        foreach(Transform child in selectedFichasContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(var ficha in currentSelectedFichas)
        {
            ficha.Owner = player;
            player.fichas.Add(ficha);
        }
        currentPlayerIndex++;

        if(currentPlayerIndex < gameData.Players)
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
        CoordsPanel.SetActive(true);
        TurnsAndSkillsCanvas.SetActive(true);
        Debug.Log("Todos los jugadores han seleccionado sus fichas. Comienza el juego.");
        foreach(var player in players)
        {
            Debug.Log($"Jugador: {player.name}");
            foreach(var ficha in player.fichas)
            {
                Debug.Log($"Ficha: {ficha.label}, Velocidad: {ficha.speed}, Recarga: {ficha.cooldown}");
                ficha.isChosen = true;
            }
        }
        if(mazeController != null && gameContext != null)
        {
            MazeController.Instance.PlaceFichasInMaze();
        }
        GameContext.Instance.gameStarted = true;
        TurnManager.Instance.StartTurn(GameContext.Instance.players[0]);
    }
}