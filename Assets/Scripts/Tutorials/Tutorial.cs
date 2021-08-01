using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject keyboardDescription;
    [SerializeField] GameObject gamepadDescription;

    GameObject activeDescription;
    
    public void StartTutorial()
    {
        // Make this active
        gameObject.SetActive(true);

        // Close active tutorial
        if (activeDescription != null) activeDescription.SetActive(false);

        // Check current device
        string currDevice = InputManager.Singleton.GetCurrentDevice();

        print("tut device: " + currDevice);

        // Show appropriate tutorial depending on device
        if (currDevice == "keyboard")
        {
            keyboardDescription.SetActive(true);
            activeDescription = keyboardDescription;
        }
        else if (currDevice == "gamepad")
        {
            gamepadDescription.SetActive(true);
            activeDescription = gamepadDescription;
        }

        // Show the tutorial
        TutorialUI.Singleton.ShowPopUp();
    }

    public void CompletedTutorial()
    {
        TutorialUI.Singleton.HidePopUp();

        Invoke("TurnThisOff", 0.8f);
    }

    void TurnThisOff()
    {
        gameObject.SetActive(false);
    }
}
