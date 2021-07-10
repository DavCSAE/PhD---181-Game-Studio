using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [HideInInspector]
    Player player;

    Animator anim;

    public Transform rootBone;

    private void OnEnable()
    {
        PlayerEvents.AttackEvent += Attack1Animation;
    }

    private void OnDisable()
    {
        PlayerEvents.AttackEvent -= Attack1Animation;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRunAnimation();
        HandleJumpAnimation();
        HandleWingFlapAnimation();
        HandleFallingAnimation();
        HandleDashAnimation();
        HandleSpawnAnimation();
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

    void HandleWingFlapAnimation()
    {
        if (player.movement.isFlapping)
        {
            anim.SetBool("startFlap", true);
            anim.SetBool("isFlapping", true);
        }
        else
        {
            anim.SetBool("isFlapping", false);
        }
    }

    public void StartedFlapping()
    {
        anim.SetBool("startFlap", false);
    }


    void Attack1Animation()
    {
        anim.SetBool("attack1", true);
    }

    public void Attacked()
    {
        anim.SetBool("attack1", false);
    }

    void HandleSpawnAnimation()
    {
        if (player.spawning.GetPreparingToSpawnState())
        {
            anim.SetBool("isPreparingToSpawn", true);
        }
        else
        {
            anim.SetBool("isPreparingToSpawn", false);
        }

        if (player.spawning.GetSpawningState())
        {
            anim.SetBool("isSpawning", true);
        }
        else
        {
            anim.SetBool("isSpawning", false);
        }
    }

    public void StartReceiveItemAnim()
    {
        anim.SetBool("isReceivingItem", true);
    }

    public void StopReceiveItemAnim()
    {
        anim.SetBool("isReceivingItem", false);
    }

}
