using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasTriggered = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !hasTriggered)
        {
            TriggerDialogue();
            hasTriggered = true;
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}