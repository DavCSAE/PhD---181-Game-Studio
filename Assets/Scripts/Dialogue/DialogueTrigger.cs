using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] DialogueData dialogue;


    [SerializeField] UnityEvent startDialogueEvent;
    [SerializeField] List<UnityEvent> afterSentenceEvents = new List<UnityEvent>();

    
    public void TriggerDialogue()
    {
        // If there is an event at the start of dialogue, then trigger it
        startDialogueEvent?.Invoke();

        DialogueManager.Singleton.StartDialogue(dialogue, this);
    }

    public void TriggerAfterSenteceEvent(int index)
    {
        // If there is an event after current sentence, then trigger it
        afterSentenceEvents[index]?.Invoke();
    }

    public DialogueData GetDialogueData()
    {
        return dialogue;
    }
}
