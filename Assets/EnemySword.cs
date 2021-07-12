using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    Enemy thisEnemy;

    public BoxCollider coll;

    EnemyCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCombatManager(EnemyCombat enemyCombat)
    {
        combat = enemyCombat;
    }

    public void ActivateCollider()
    {
        coll.enabled = true;
    }

    public void DeactivateCollider()
    {
        print("collider deactivated");
        coll.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        print("hit " + other.name);

        if (other.GetComponent<PlayerStats>())
        {
            combat.HitPlayer(other.GetComponent<PlayerStats>());
        }

    }
}
