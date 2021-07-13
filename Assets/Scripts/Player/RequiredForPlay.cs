using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredForPlay : MonoBehaviour
{
    GameObject requiredForPlay;

    // Input Manager
    [SerializeField] GameObject inputManagerPrefab;
    GameObject inputManagerObject;

    // Camera
    [SerializeField] Transform camTarget;
    [SerializeField] GameObject freeLookCinemachinePrefab;
    public GameObject freeLookCinemachineObj;

    // Dialogue System
    [SerializeField] GameObject dialogueManagerPrefab;
    GameObject dialogueSystem;

    // Interaction UI
    [SerializeField] GameObject interactionUI;

    //BlackScreen
    [SerializeField] GameObject blackScreen;

    //Pause Menu UI
    [SerializeField] GameObject pauseMenuUI;

    // In game HUD UI
    [SerializeField] GameObject hudUI;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnRequiredObjects();

        // If there's no InputManager then this must be for testing
        if (InputManager.Singleton == null)
        {
            SpawnInputManager();
        }

        SpawnCameras();

        SpawnDialogueSystem();

        SpawnInteractionUI();

        if (BlackScreen.Singleton == null)
        {
            SpawnBlackScreen();
        }

        SpawnPauseMenuUI();

        SpawnHUDUI();
    }

    void SpawnRequiredObjects()
    {
        // Create test kit object to hold test kit
        requiredForPlay = new GameObject("Required For Play");
        // Make test kit child of player object
        requiredForPlay.transform.parent = transform;
        // Reset test kit transforms
        requiredForPlay.transform.localPosition = Vector3.zero;
    }

    void SpawnInputManager()
    {
        // Spawn InputManager
        inputManagerObject = Instantiate(inputManagerPrefab, requiredForPlay.transform);
        inputManagerObject.GetComponent<InputManager>().EnableInputs();
    }

    void SpawnCameras()
    {
        // Spawn Cinemachine Camera
        freeLookCinemachineObj = Instantiate(freeLookCinemachinePrefab, requiredForPlay.transform);
        // Setup Cinemachine Camera
        Cinemachine.CinemachineFreeLook cinemachineFreeLook = freeLookCinemachineObj.GetComponent<Cinemachine.CinemachineFreeLook>();
        cinemachineFreeLook.Follow = transform;
        cinemachineFreeLook.LookAt = camTarget;
    }

    void SpawnDialogueSystem()
    {
        dialogueSystem = Instantiate(dialogueManagerPrefab, requiredForPlay.transform);
    }

    void SpawnInteractionUI()
    {
        Instantiate(interactionUI, requiredForPlay.transform);
    }

    void SpawnBlackScreen()
    {
        Instantiate(blackScreen);
    }

    void SpawnPauseMenuUI()
    {
        Instantiate(pauseMenuUI, requiredForPlay.transform);
    }

    void SpawnHUDUI()
    {
        Instantiate(hudUI, requiredForPlay.transform);
    }
}
