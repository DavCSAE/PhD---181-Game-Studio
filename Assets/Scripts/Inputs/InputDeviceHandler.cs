using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceHandler : MonoBehaviour
{
    // Player Input
    PlayerInput playerInput;

    string currentScheme;

    // Start is called before the first frame update
    void Start()
    {
        SetUpPlayerInput();
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeviceChange();
    }

    void SetUpPlayerInput()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions = InputManager.Singleton.playerControls.asset;

        currentScheme = playerInput.currentControlScheme;

        playerInput.CancelInvoke();
    }

    void HandleDeviceChange()
    {
        if (currentScheme != playerInput.currentControlScheme)
        {
            currentScheme = playerInput.currentControlScheme;

            print("Changed to " + currentScheme);

            InputManager.Singleton.SetCurrentDevice(currentScheme);
            PlayerEvents.TriggerInputDeviceChangeEvent();
        }
    }



}
