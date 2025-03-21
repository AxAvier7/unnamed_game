using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour//este script se encarga de manejar el dialogo corto
{
    public Text nameText;
    public Text dialogueText;
    public GameObject formPanel;
    public GameObject interaction;
    public Dialogue dialogue;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        formPanel.SetActive(false);
        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.Name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

            DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            ShowForm();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void ShowForm()
    {
        formPanel.SetActive(true);
        interaction.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (sentences.Count > 0)
            {
                DisplayNextSentence();
            }
            else
            {
                ShowForm();
            }
        }
    }
}
