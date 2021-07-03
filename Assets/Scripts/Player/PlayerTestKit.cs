using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestKit : MonoBehaviour
{
    GameObject testKit;

    // Input Manager
    [SerializeField] GameObject inputManagerPrefab;
    GameObject inputManagerObject;

    // Camera
    [SerializeField] Transform camTarget;
    [SerializeField] GameObject freeLookCinemachinePrefab;
    GameObject freeLookCinemachineObj;

    // Dialogue System
    [SerializeField] GameObject DialogueManagerPrefab;
    GameObject dialogueSystem;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnTestKit();

        // If there's no InputManager then this must be for testing
        if (InputManager.Singleton == null)
        {
            SpawnInputManager();
        }

        SpawnCameras();

        SpawnDialogueSystem();
    }

    void SpawnTestKit()
    {
        // Create test kit object to hold test kit
        testKit = new GameObject("Test Kit");
        // Make test kit child of player object
        testKit.transform.parent = transform;
        // Reset test kit transforms
        testKit.transform.localPosition = Vector3.zero;
    }

    void SpawnInputManager()
    {
        // Spawn InputManager
        inputManagerObject = Instantiate(inputManagerPrefab, testKit.transform);
        inputManagerObject.GetComponent<InputManager>().EnableInputs();
    }

    void SpawnCameras()
    {
        // Spawn Cinemachine Camera
        freeLookCinemachineObj = Instantiate(freeLookCinemachinePrefab, testKit.transform);
        // Setup Cinemachine Camera
        Cinemachine.CinemachineFreeLook cinemachineFreeLook = freeLookCinemachineObj.GetComponent<Cinemachine.CinemachineFreeLook>();
        cinemachineFreeLook.Follow = transform;
        cinemachineFreeLook.LookAt = camTarget;
    }

    void SpawnDialogueSystem()
    {
        dialogueSystem = Instantiate(DialogueManagerPrefab, testKit.transform);
    }
}
