using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;

    bool canAttack;

    [SerializeField] bool isAttacking;

    [SerializeField] bool inSwing1;
    [SerializeField] bool inSwing2;
    [SerializeField] bool canChainSwing;

    [SerializeField] PlayerSword sword;

    [SerializeField] Animator slashAnim;



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
        if (!canAttack) return;

        // Don't attack if currently attacking
        if (isAttacking && !canChainSwing) return;

        isAttacking = true;

        if (!inSwing1)
        {
            FirstSwing();
        }
        else if (!inSwing2)
        {
            SecondSwing();
        }


        // Play sound
        PlaySwordSwingSound();

        // Enable sword collider
        sword.ActivateCollider();
    }

    void FirstSwing()
    {
        if (inSwing2) player.animations.Attacked2();

        // Play animation
        player.animations.Attack1Animation(FinishedAttack);

        // Play Slash effect
        slashAnim.SetTrigger("attack");

        // Update state
        inSwing1 = true;
        inSwing2 = false;
    }

    void SecondSwing()
    {
        if (inSwing1) player.animations.Attacked1();

        // Play animation
        player.animations.Attack2Animation(FinishedAttack);

        // Play Slash effect
        //slashAnim.SetTrigger("attack");

        // Update states
        inSwing2 = true;
        inSwing1 = false;
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

    void FinishedAttack()
    {
        isAttacking = false;

        sword.DeactivateCollider();

        if (inSwing1) Invoke("FinishedFirstAttack", 0.5f);
        if (inSwing2) Invoke("FinishedSecondAttack", 0.5f);
    }


    void FinishedFirstAttack()
    {
        inSwing1 = false;

        CantChainSwing();
    }

    void FinishedSecondAttack()
    {
        inSwing2 = false;

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
