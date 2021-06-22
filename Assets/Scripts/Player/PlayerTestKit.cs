using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestKit : MonoBehaviour
{
    bool isTesting;

    GameObject testKit;

    [SerializeField] GameObject inputManagerPrefab;
    GameObject inputManagerObject;

    [SerializeField] Transform camTarget;
    [SerializeField] GameObject freeLookCinemachinePrefab;
    GameObject freeLookCinemachineObj;


    // Start is called before the first frame update
    void Awake()
    {
        // If there's no InputManager then this must be for testing
        if (InputManager.Singleton == null)
        {
            isTesting = true;
        }

        if (isTesting)
        {
            SpawnTestKit();
        }
    }

    void SpawnTestKit()
    {
        // Create test kit object to hold test kit
        testKit = new GameObject("Test Kit");
        // Make test kit child of player object
        testKit.transform.parent = transform;
        // Reset test kit transforms
        testKit.transform.localPosition = Vector3.zero;

        // Spawn InputManager
        inputManagerObject = Instantiate(inputManagerPrefab, testKit.transform);
        inputManagerObject.GetComponent<InputManager>().EnableInputs();
    
        // Spawn Cinemachine Camera
        freeLookCinemachineObj = Instantiate(freeLookCinemachinePrefab, testKit.transform);
        // Setup Cinemachine Camera
        Cinemachine.CinemachineFreeLook cinemachineFreeLook = freeLookCinemachineObj.GetComponent<Cinemachine.CinemachineFreeLook>();
        cinemachineFreeLook.Follow = transform;
        cinemachineFreeLook.LookAt = camTarget;

    }
}
