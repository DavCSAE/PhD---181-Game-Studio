using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerButtonSystem : MonoBehaviour
{
    [SerializeField]
    List<Button> buttons = new List<Button>();

    Button currentButton;

    bool isInputLeft;
    bool isInputUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleButtonSelection();
    }
    void HandleButtonSelection()
    {
        //Getting the direction of the input
        Vector2 moveInput = InputManager.Singleton.GetMoveInput();

        //Don't do anything if there is no directional input
        if (moveInput == Vector2.zero) return;

        //Setting the boolean values based on the direction of input
        if (moveInput.x < 0) isInputLeft = true;
        if (moveInput.y > 0) isInputUp = true;

        //Make a new list to store the buttons in the direction of the input
        List<Button> buttonsInDirection = new List<Button>();

        foreach(Button button in buttons)
        {

        }

    }
}
