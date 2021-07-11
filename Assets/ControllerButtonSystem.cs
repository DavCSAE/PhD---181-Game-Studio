using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerButtonSystem : MonoBehaviour
{
    [SerializeField]
    List<Button> buttons = new List<Button>();

    [SerializeField]
    Button currentButton;

    bool isInputLeft;
    bool isInputUp;

    enum HorizontalDirection
    {
        None,
        Left,
        Right,
    }

    HorizontalDirection horDir;
    enum VerticalDirection
    {
        None,
        Up,
        Down,
    }

    VerticalDirection verDir;

    // Start is called before the first frame update
    void Start()
    {
        currentButton.GetComponent<Animator>().SetTrigger("Highlighted");
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
        //if (moveInput.x < 0) isInputLeft = true;
        //if (moveInput.y > 0) isInputUp = true;

        if (moveInput.x == 0) horDir = HorizontalDirection.None;
        if (moveInput.x < 0) horDir = HorizontalDirection.Left;
        if (moveInput.x > 0) horDir = HorizontalDirection.Right;

        if (moveInput.y == 0) verDir = VerticalDirection.None;
        if (moveInput.y < 0) verDir = VerticalDirection.Up;
        if (moveInput.y > 0) verDir = VerticalDirection.Down;


        //Make a new list to store the buttons in the direction of the input
        List<Button> buttonsInDirection = new List<Button>();

        foreach (Button button in buttons)
        {
            switch (horDir)
            {
                case HorizontalDirection.None:
                    //do nothing
                    break;
                case HorizontalDirection.Left:
                    //Check if button is to the left of the current button
                    if (button.transform.position.x < currentButton.transform.position.x)
                    {
                        //button is to the left
                    }
                    else
                    {
                        //button isn't in input direction so do nothing
                        return;
                    }
                    break;
                case HorizontalDirection.Right:
                    //Check if button is to the right of the current button
                    if (button.transform.position.x > currentButton.transform.position.x)
                    {
                        //button is to the right
                    }
                    else
                    {
                        //button isn't in input direction so do nothing
                        return;
                    }
                    break;
            }
            switch (verDir)
            {
                case VerticalDirection.None:
                    //do nothing
                    break;
                case VerticalDirection.Up:
                    //Check if button is above the current button
                    if (button.transform.position.y > currentButton.transform.position.y)
                    {
                        //button is above
                        buttonsInDirection.Add(button);
                    }
                    else
                    {
                        //button isn't in input direction so do nothing
                        return;
                    }
                    break;
                case VerticalDirection.Down:
                    //Check if button is below the current button
                    if (button.transform.position.y < currentButton.transform.position.y)
                    {
                        //button is below
                        buttonsInDirection.Add(button);
                    }
                    else
                    {
                        //button isn't in input direction so do nothing
                        return;
                    }
                    break;
            }
        }

        //If there are no buttons in the input direction then return
        if (buttonsInDirection.Count == 0) return;

        Button currentClosestButton = buttonsInDirection[0];

        //find closest button of all buttons in input direction
        for (int i = 0; i < buttonsInDirection.Count; i++)
        {
            if (Vector2.Distance(currentButton.transform.position, currentClosestButton.transform.position)
                 > Vector2.Distance(currentButton.transform.position, buttonsInDirection[i].transform.position))
            {
                currentClosestButton = buttonsInDirection[i];
            }
        }

        ChangeCurrentButton(currentClosestButton);
    }


    void ChangeCurrentButton(Button button)
    {
        //return previously selected button to normal state
        currentButton.GetComponent<Animator>().SetTrigger("Normal");

        //set currently selected button to new button
        currentButton = button;

        //set new button to highlighted state
        currentButton.GetComponent<Animator>().SetTrigger("Highlighted");
    }
}
