using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueForm : MonoBehaviour //clase asociada al formulario que el usuario rellena para luego crear el laberinto
{
    public Text dialogueText;
    public InputField inputField;
    public Button submitButton;

    private int currentStep = 0;

    void Start()
    {
        ShowQuestion();
        submitButton.onClick.AddListener(HandleSubmit);
    }

    void ShowQuestion()//metodo que muestra las preguntas que se la hacen al usuario para que este rellene con parametros
    {
        switch (currentStep)
        {
            case 0:
                dialogueText.text = "Buenas querido usuario. ¿Cuántos aventureros serán hoy?";
                inputField.text = "";
                break;
            case 1:
                dialogueText.text = $"Jugador {GameData.Instance.PlayerNames.Count + 1}, ingresa tu nombre:";
                inputField.text = "";
                break;
            case 2:
                dialogueText.text = "¿Con cuántas fichas desean jugar?";
                inputField.text = "";
                break;
            case 3:
                dialogueText.text = "¡Gracias! Disfruten su aventura.";
                inputField.gameObject.SetActive(false);
                submitButton.gameObject.SetActive(false);
                Invoke("LoadSceneFour", 2f);
                break;
        }
    }

    void HandleSubmit()// metodo con el que se guarda la informacion que introduce el usuario y que indica si hay errores en el inputs
    {
        string input = inputField.text.Trim();

        switch (currentStep)
        {
            case 0:
                if (int.TryParse(input, out int playersInput))
                {
                    if (playersInput > 4 || playersInput == 1)
                    {
                        dialogueText.text = "No pueden haber más de 4 jugadores. Introduce un número entre 4 y 2.";
                        return;
                    }
                    GameData.Instance.Players = playersInput;
                    currentStep++;
                }
                else
                {
                    dialogueText.text = "Por favor, introduce un número válido.";
                }
                break;

            case 1:
                if(!string.IsNullOrEmpty(input))
                {
                    GameData.Instance.PlayerNames.Add(input);
                    if (GameData.Instance.PlayerNames.Count < GameData.Instance.Players)
                    {
                        ShowQuestion();
                        return;
                    }
                    currentStep++;
                }
                else
                {
                    dialogueText.text = "Por favor, introduce un nombre válido.";
                }
                break;

            case 2:
                if(int.TryParse(input, out int chipsInput))
                {
                    if (chipsInput > 5 || chipsInput < 1)
                    {
                        dialogueText.text = "No admitimos más de 5 fichas. Introduce una cantidad de fichas menor a 5.";
                        return;
                    }
                    GameData.Instance.Chips = chipsInput;
                    currentStep++;
                }
                else
                {
                    dialogueText.text = "Por favor, introduce un número válido.";
                }
                break;
        }

        ShowQuestion();
    }

    void LoadSceneFour()
    {
        SceneManager.LoadScene(4);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResetForm();
        }
    }

    public void ResetForm()
    {
        currentStep = 0;
        GameData.Instance.Players = 0;
        GameData.Instance.PlayerNames.Clear();
        GameData.Instance.Chips = 0;
        ShowQuestion();
    }

}