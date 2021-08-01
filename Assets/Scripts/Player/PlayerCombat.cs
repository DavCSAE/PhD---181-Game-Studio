using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;

    bool canAttack;

    bool isAttacking;

    [SerializeField] PlayerSword sword;

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
        if (isAttacking) return;

        isAttacking = true;

        // Play animation
        player.animations.Attack1Animation(FinishedAttack);

        // Play sound
        PlaySwordSwingSound();

        // Enable sword collider
        sword.ActivateCollider();
    }

    // Animation event
    public void PlaySwordSwingSound()
    {
        SoundManager.Singleton.Play("Sword Swing");
    }

    void FinishedAttack()
    {
        isAttacking = false;

        sword.DeactivateCollider();
    }

    public void HitEnemy(Enemy hitEnemy)
    {
        hitEnemy.TakeDamage(1);
    }

}
