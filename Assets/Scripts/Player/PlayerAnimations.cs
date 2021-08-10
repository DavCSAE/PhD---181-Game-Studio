using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [HideInInspector]
    Player player;

    Animator anim;

    public Transform rootBone;

    public delegate void AttackFinishedCallback();
    AttackFinishedCallback attackFinishedCallback;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
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
        HandleTargetingMovementAnimation();

        HandleAttackAnimations();
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


    void HandleAttackAnimations()
    {
        anim.SetBool("attack1", player.combat.inSwing1);
        anim.SetBool("attack2", player.combat.inSwing2);

        if (!player.combat.isAttacking) player.combat.canChainSwing = false;

        anim.SetBool("canChainAttack", player.combat.canChainSwing);
    }

    public void Attack1Animation(AttackFinishedCallback callback)
    {
        //anim.SetBool("attack1", true);

        //attackFinishedCallback = callback;
    }

    public void Attack2Animation(AttackFinishedCallback callback)
    {
        //anim.SetBool("attack2", true);

        //attackFinishedCallback = callback;
    }

    public void StartedAttack1Event()
    {
        player.combat.StartSlash1Effect();
    }

    public void StartedAttack2Event()
    {
        player.combat.StartSlash2Effect();
    }

    public void Attacked1()
    {
        //anim.SetBool("attack1", false);

        //attackFinishedCallback();
        player.combat.FinishedAttack();
    }

    public void Attacked2()
    {
        //anim.SetBool("attack2", false);

        //attackFinishedCallback();
        player.combat.FinishedAttack();
    }

    public void CanChainAttack()
    {
        player.combat.CanChainSwing();

        //anim.SetBool("canChainAttack", true);
    }

    public void SetCanChangeAttack(bool state)
    {
        //anim.SetBool("canChainAttack", state);
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

    public void FootstepAnimEvent()
    {
        //SoundManager.Singleton.Play("Footstep");
    }

    void HandleTargetingMovementAnimation()
    {
        if (!player.targeting.isTargeting)
        {
            if (anim.GetFloat("yRunning") != 1) anim.SetFloat("yRunning", 1);
            if (anim.GetFloat("xRunning") != 0) anim.SetFloat("xRunning", 0);

            return;
        }

        print("targeting movement");

        float x = 0;
        float y = 0;

        Vector2 moveDir = InputManager.Singleton.GetMoveInput();

        print("movDir: " + moveDir);

        if (moveDir != Vector2.zero)
        {
            float angle = Vector2.Angle(moveDir, Vector2.up);

            print("angle: " + angle);

            if (angle >= 0 && angle < 45)
            {
                y = 1;

            }
            else if (angle > 45 && angle < 135)
            {
                float rightAngle = Vector2.Angle(moveDir, Vector2.right);
                float leftAngle = Vector2.Angle(moveDir, Vector2.left);

                if (rightAngle < leftAngle)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            else if (angle > 135)
            {
                y = -1;
            }
        }
        print("x: " + x);
        print("y: " + y);

        anim.SetFloat("yRunning", y);
        anim.SetFloat("xRunning", x);
    }
}
