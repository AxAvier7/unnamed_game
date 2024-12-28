using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueForm : MonoBehaviour
{
    public Text dialogueText;
    public InputField inputField;
    public Button submitButton;

    private int currentStep = 0;
    private int players = 0;
    private List<string> playerNames = new List<string>();
    private int chips = 0;

    void Start()
    {
        ShowQuestion();
        submitButton.onClick.AddListener(HandleSubmit);
    }

    void ShowQuestion()
    {
        switch (currentStep)
        {
            case 0:
                dialogueText.text = "Buenas querido usuario. ¿Cuántos aventureros serán hoy?";
                inputField.text = "";
                break;
            case 1:
                dialogueText.text = $"Jugador {playerNames.Count + 1}, ingresa tu nombre:";
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
                break;
        }
    }

    void HandleSubmit()
    {
        string input = inputField.text;

        switch (currentStep)
        {
            case 0:
                players = int.Parse(input);
                if (players > 4 || players == 1)
                {
                    dialogueText.text = "No pueden haber más de 4 jugadores. Introduce un número de jugadores menor a 4 y distinto de 1.";
                    return;
                }
                currentStep++;
                break;

            case 1:
                playerNames.Add(input);
                if (playerNames.Count < players)
                {
                    ShowQuestion();
                    return;
                }
                currentStep++;
                break;

            case 2:
                chips = int.Parse(input);
                if (chips > 5)
                {
                    dialogueText.text = "No admitimos más de 5 fichas. Introduce una cantidad de fichas menor a 5.";
                    return;
                }
                currentStep++;
                break;
        }

        ShowQuestion();
    }
}
