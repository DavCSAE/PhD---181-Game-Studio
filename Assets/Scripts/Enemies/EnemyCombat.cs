using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    Enemy thisEnemy;

    [SerializeField] EnemySword sword;

    [SerializeField] bool hasSword;

    [SerializeField] bool hasStartedAttacking;
    bool inCombat;
    public bool currentlyAttacking;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<Enemy>();
        sword.SetCombatManager(this);

        if (hasSword) ActivateSword();

        
    }

    // Update is called once per frame
    void Update()
    {
        //if (hasStartedAttacking) StartAttacking();

        //if (inCombat && !currentlyAttacking) Attack();
    }

    public void ActivateSword()
    {
        sword.gameObject.SetActive(true);
    }

    public void StartAttacking()
    {


        GetComponent<ShadeIdleAnimations>().StopIdling();

        Attack();
    }

    public void Attacked1()
    {
        sword.DeactivateCollider();
        currentlyAttacking = false;

        thisEnemy.navigation.StartNavigating();

    }

    void Attack()
    {
        // Don't attack if currently attacking
        if (currentlyAttacking) return;

        inCombat = true;
        currentlyAttacking = true;

        // Play animation
        thisEnemy.anim.SetBool("attack1", true);

        // Enable sword collider
        sword.ActivateCollider();
    }

    void FinishedAttack()
    {

        sword.DeactivateCollider();
    }

    public void HitPlayer(PlayerStats hitPlayer)
    {
        hitPlayer.TakeDamage(1);
    }

    public void StartedAttack1Event()
    {
        thisEnemy.anim.SetBool("attack1", false);
    }
}
