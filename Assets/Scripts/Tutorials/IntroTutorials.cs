using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTutorials : MonoBehaviour
{
    // Movement Tutorial
    [SerializeField] Tutorial movementTutorial;
    bool movementTutorialActive;

    // Jumping Tutorial
    [SerializeField] Tutorial jumpingTutorial;
    bool jumpingTutorialActive;

    // High Jumping Tutorial
    [SerializeField] Tutorial highJumpingTutorial;
    bool highJumpingTutorialActive;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementTutorial();
        HandleJumpingTutorial();
    }

    public void ActivateMovementTutorial()
    {
        movementTutorialActive = true;

        movementTutorial.StartTutorial();
    }

    void HandleMovementTutorial()
    {
        // If movement tutorial is not active, don't do anything
        if (!movementTutorialActive) return;

        // Ff player has used the movement controls, tutorial is complete
        if (InputManager.Singleton.GetMoveInput() != Vector2.zero)
        {
            movementTutorialActive = false;

            movementTutorial.CompletedTutorial();
        }
    }

    public void ActivateJumpingTutorial()
    {
        jumpingTutorialActive = true;

        jumpingTutorial.StartTutorial();
    }

    void HandleJumpingTutorial()
    {
        // If movement tutorial is not active, don't do anything
        if (!jumpingTutorialActive) return;

        // Ff player has used the movement controls, tutorial is complete
        if (InputManager.Singleton.GetJumpInput())
        {
            jumpingTutorialActive = false;

            jumpingTutorial.CompletedTutorial();
        }
    }

    public void ActivateHighJumpingTutorial()
    {
        highJumpingTutorialActive = true;

        highJumpingTutorial.StartTutorial();
    }

    void HandleHighJumpingTutorial()
    {
        // If movement tutorial is not active, don't do anything
        if (!highJumpingTutorialActive) return;

        // Ff player has used the movement controls, tutorial is complete
        if (InputManager.Singleton.GetJumpInput())
        {
            highJumpingTutorialActive = false;

            highJumpingTutorial.CompletedTutorial();
        }
    }

    public void CompletedHighJumpingTutorial()
    {
        highJumpingTutorialActive = false;

        highJumpingTutorial.CompletedTutorial();
    }

}
