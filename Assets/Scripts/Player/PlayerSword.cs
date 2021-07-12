using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    PlayerCombat combat;

    public BoxCollider coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerCombatManager(PlayerCombat playerCombat)
    {
        combat = playerCombat;
    }

    public void ActivateCollider()
    {
        print("collider activated");
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

        if (other.GetComponent<Enemy>())
        {
            combat.HitEnemy(other.GetComponent<Enemy>());
        }
        
    }
}
