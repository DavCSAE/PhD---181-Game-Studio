using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    EnemyStats stats;
    EnemyCombat combat;

    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        combat = GetComponent<EnemyCombat>();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        stats.TakeDamage(amount);
    }
}
