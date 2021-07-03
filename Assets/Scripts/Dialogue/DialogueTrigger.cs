using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] DialogueData dialogue;
    public void TriggerDialogue()
    {
        DialogueManager.Singleton.StartDialogue(dialogue);
    }
}
