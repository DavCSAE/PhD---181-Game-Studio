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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        // Don't attack if currently attacking
        if (isAttacking) return;

        isAttacking = true;

        // Play animation
        player.animations.Attack1Animation(FinishedAttack);

        // Enable sword collider
        sword.ActivateCollider();
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
