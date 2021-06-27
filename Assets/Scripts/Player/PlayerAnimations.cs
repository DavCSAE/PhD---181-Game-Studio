using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    Animator anim;

    public Transform rootBone;

    // STATES

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRunAnimation();
        HandleJumpAnimation();
        HandleFallingAnimation();
        HandleDashAnimation();
    }

    void HandleRunAnimation()
    {
        if (player.movement.isGrounded && player.movement.isMoving)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void HandleJumpAnimation()
    {
        if (player.movement.isJumping)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }

    void HandleFallingAnimation()
    {
        if (player.movement.isFalling)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    void HandleDashAnimation()
    {
        if (player.movement.isDashing)
        {
            anim.SetBool("isDashing", true);
        }
        else
        {
            anim.SetBool("isDashing", false);
        }
    }

}
