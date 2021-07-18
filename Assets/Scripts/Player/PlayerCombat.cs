using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;

    bool canAttack;

    public bool isAttacking;

    public bool inSwing1;
    public bool inSwing2;
    public bool canChainSwing;

    [SerializeField] PlayerSword sword;

    [SerializeField] Animator slashAnim;
    [SerializeField] Animator slash2Anim;

    public bool isProjectileSlash;
    public float projectileRange = 1f;



    void OnEnable()
    {
        PlayerEvents.AttackEvent += Attack;
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        sword.SetPlayerCombatManager(this);

        if (player.isSwordUnlocked)
        {
            canAttack = true;
            ShowSword();
        }
        else
        {
            HideSword();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CleanAttackStates();
    }
    public void UnlockSword()
    {
        canAttack = true;
        ShowSword();
    }

    public void ShowSword()
    {
        sword.gameObject.SetActive(true);
    }

    public void HideSword()
    {
        sword.gameObject.SetActive(false);
    }

    void Attack()
    {

        // If cannot attack, return
        if (!canAttack) return;

        // If attacking and can't chain attack, return
        if (isAttacking && !canChainSwing) return;

        // Start attacking
        isAttacking = true;

        // If not in swing1, then do swing 1
        if (!inSwing1)
        {
            FirstSwing();
            Invoke("CantChainSwing", 0.05f);
        }
        // If in swing 1 and not in swing 2, do swing 2
        else if (!inSwing2)
        {
            SecondSwing();
            Invoke("CantChainSwing", 0.05f);
        }


        // Play sound
        PlaySwordSwingSound();

        // Enable sword collider
        sword.ActivateCollider();
    }

    void CleanAttackStates()
    {
        if (!isAttacking)
        {
            inSwing1 = false;
            inSwing2 = false;
            canChainSwing = false;
        }
    }

    void FirstSwing()
    {
        // If in swing2, turn off attacked 2 animations
        //if (inSwing2) player.animations.Attacked2();

        // Play animation swing1 animation
        //player.animations.Attack1Animation(FinishedAttack);


        // Update state
        inSwing1 = true;
        inSwing2 = false;

    }

    void SecondSwing()
    {
        // If in swing1, turn off attacked 1 animations
        //if (inSwing1) player.animations.Attacked1();

        // Play animation
        //player.animations.Attack2Animation(FinishedAttack);


        // Update states
        inSwing2 = true;
        inSwing1 = false;

    }

    public void StartSlash1Effect()
    {

        // Play Slash effect
        slashAnim.SetTrigger("attack");
    }

    public void StartSlash2Effect()
    {

        // Play Slash effect
        slash2Anim.SetTrigger("attack2");
    }

    public void CanChainSwing()
    {
        canChainSwing = true;
    }

    // Animation event
    public void PlaySwordSwingSound()
    {
        if (SoundManager.Singleton)
        {
            SoundManager.Singleton.Play("Sword Swing");
        }
        
    }

    public void FinishedAttack()
    {
        

        sword.DeactivateCollider();

        if (inSwing1) Invoke("FinishedFirstAttack", 0.25f);
        if (inSwing2) Invoke("FinishedSecondAttack", 0.25f);
    }


    void FinishedFirstAttack()
    {
        inSwing1 = false;
        isAttacking = false;

        CantChainSwing();
    }

    void FinishedSecondAttack()
    {
        inSwing2 = false;
        isAttacking = false;

        CantChainSwing();
    }

    void CantChainSwing()
    {
        print("cant");
        canChainSwing = false;
        player.animations.SetCanChangeAttack(false);
    }

    public void HitEnemy(Enemy hitEnemy)
    {
        hitEnemy.TakeDamage(1);
    }

}
