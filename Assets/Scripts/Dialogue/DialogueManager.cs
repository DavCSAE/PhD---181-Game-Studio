using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Singleton;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    Animator anim;

    private Queue<string> sentences;

    [SerializeField] float timeBetweenLetters = 0.1f;

    string currentSentence;
    bool sentenceFinished = true;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogue)
    {
        // Show Dialogue UI on screen
        anim.SetBool("IsOpen", true);

        // Set the name text to the name of the NPC speaking
        nameText.text = dialogue.npcName;

        // Clear any leftover sentences
        sentences.Clear();

        // Add each sentence in the dialogue to a queue
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Add DisplayNextSentence function to NextDialogue event
        PlayerEvents.NextDialogueEvent += DisplayNextSentence;

        // Trigger event
        PlayerEvents.TriggerStartDialogueEvent();

        // Display the first sentence
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // If there are no more sentences, then end the dialogue
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // If a sentence is still printing, then complete that sentence instead of
        // showing next sentence.
        if (!sentenceFinished)
        {
            dialogueText.text = currentSentence;
            sentenceFinished = true;
            StopAllCoroutines();
            return;
        }

        // Get the next sentence from the queue
        currentSentence = sentences.Dequeue();

        // Stop typing a sentence if one is currently being typed
        StopAllCoroutines();

        // Sentence isn't finished since it is starting to print
        sentenceFinished = false;

        // Start typing the next sentence
        StartCoroutine(TypeSentence(currentSentence));
    }    

    IEnumerator TypeSentence(string sentence)
    {
        // Empty the dialogue text
        dialogueText.text = "";

        // Add each letter in the sentence to the dialogue text, one at a time
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if (dialogueText.text == sentence) sentenceFinished = true;

            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    void EndDialogue()
    {
        // Close the dialogue UI
        anim.SetBool("IsOpen", false);

        // Remove DisplayNextSentence function from NextDialogue event
        PlayerEvents.NextDialogueEvent -= DisplayNextSentence;

        // Trigger event
        PlayerEvents.TriggerEndDialogueEvent();
    }
}
