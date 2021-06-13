using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    Animator anim;

    // STATES
    bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRunAnimation();
    }

    void HandleRunAnimation()
    {
        if (player.movement.isGrounded && player.movement.isMoving)
        {
            SetRunningState(true);
        }
        else if(isRunning)
        {
            SetRunningState(false);
        }
    }

    public void SetRunningState(bool newState)
    {
        isRunning = newState;

        if (isRunning)
        {
            anim.SetBool("isRunning", true);
        }
        else if (!isRunning)
        {
            anim.SetBool("isRunning", false);
        }
    }

    public bool GetRunningState()
    {
        return isRunning;
    }
}
