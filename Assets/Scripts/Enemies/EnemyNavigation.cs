using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] bool isNavigating;
    NavMeshAgent navMeshAgent;

    bool canNavigate;

    [SerializeField] float rangeToAttackWithin = 1.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleNavigation();
    }

    void HandleNavigation()
    {
        if (!canNavigate) return;

        if(enemy.combat.currentlyAttacking)
        {
            StopNavigating();
        }

        float distanceToTarget = CheckDistanceToTarget();

        if (!isNavigating)
        {
            if (distanceToTarget > rangeToAttackWithin && !enemy.combat.currentlyAttacking)
            {
                StartNavigating();
            }

            return;
        }


        if (distanceToTarget <= rangeToAttackWithin)
        {
            print("enemy attack!");
            StopNavigating();
            Invoke("Attack", 1.2f);
            //enemy.combat.StartAttacking();
        }


        navMeshAgent.SetDestination(Player.Singleton.transform.position);
    }

    void Attack()
    {


        enemy.combat.StartAttacking();
    }

    public void StartNavigating()
    {
        if (CheckDistanceToTarget() <= rangeToAttackWithin) return;

        isNavigating = true;
        navMeshAgent.isStopped = false;
    }

    public void StopNavigating()
    {
        isNavigating = false;
        navMeshAgent.isStopped = true;
    }

    float CheckDistanceToTarget()
    {
        float distance = Vector3.Distance(transform.position, Player.Singleton.transform.position);

        return distance;
    }

    public void AllowNavigation()
    {
        canNavigate = true;
    }
}
