using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
public class IntroSceneBirdArrivalCutscene : MonoBehaviour
{
    [SerializeField] PlayableDirector birdArrivalCutscene;

    [SerializeField] GameObject virtualCamera;

    [SerializeField] Vector3 playerStartPos;
    [SerializeField] Vector3 playerStartRot;

    [SerializeField] Vector2 freeLookCamEndPos;

    DialogueTrigger birdArrivalDialogueTrigger;
    DialogueData birdArrivalDialogueData;

    // Start is called before the first frame update
    void Start()
    {
        birdArrivalDialogueTrigger = GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartCutscene()
    {
        // Set players position in front of bird
        SetPlayerStartPos();

        // Set players rotation to be facing bird
        SetPlayerStartRot();

        // Enable virtual camera
        virtualCamera.SetActive(true);

        // Play cutscene timeline
        birdArrivalCutscene.Play();

        BlackScreen.Singleton.FadeFromBlack();
    }

    public void FadeForCutscene()
    {
        BlackScreen.Singleton.FadeToBlack(StartCutscene, 1f);

        InputManager.Singleton.FreezeMoveInput();

        FreeLookAddOn.Singleton.Lock();
    }

    public void FadeForEndOfCutscene()
    {
        BlackScreen.Singleton.FadeToBlack(EndCutscene, 1f);
    }

    void EndCutscene()
    {

    }

    void SetPlayerStartPos()
    {
        Player.Singleton.transform.position = playerStartPos;
    }

    void SetPlayerStartRot()
    {
        Player.Singleton.transform.localEulerAngles = playerStartRot;
    }

    public void StartDialogue()
    {
        birdArrivalDialogueTrigger.TriggerDialogue();
    }

    public void PauseCutscene()
    {
        birdArrivalCutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);
        
    }

    public void UpdateName()
    {
        birdArrivalDialogueData.npcName = "Order";
    }
    
}
