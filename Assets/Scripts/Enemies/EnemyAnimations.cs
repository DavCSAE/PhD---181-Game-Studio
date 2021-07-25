using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAnimations : MonoBehaviour
{
    Enemy thisEnemy;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleSpawning();
    }

    void HandleSpawning()
    {
        if (thisEnemy.spawning.isPreparingToSpawn)
        {
            anim.SetBool("isPreparingToSpawn", true);
        }
        else
        {
            anim.SetBool("isPreparingToSpawn", false);
        }

        if (thisEnemy.spawning.isSpawning)
        {
            anim.SetBool("isSpawning", true);
        }
        else
        {
            anim.SetBool("isSpawning", false);
        }
    }

    public void CanChainAttack()
    {

    }

    public void Attacked1()
    {

    }

    public void Attacked2()
    {

    }

    public void StartedAttack1Event()
    {

    }
}
