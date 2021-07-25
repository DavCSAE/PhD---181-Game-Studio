using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool preparedForSpawning;

    public EnemyStats stats;
    public EnemyCombat combat;
    public EnemyAnimations animations;
    public EnemySpawning spawning;
    public PlayerTarget playerTarget;
    public EnemyNavigation navigation;


    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        combat = GetComponent<EnemyCombat>();
        spawning = GetComponent<EnemySpawning>();

        anim = GetComponent<Animator>();
        playerTarget = GetComponentInChildren<PlayerTarget>();
        navigation = GetComponent<EnemyNavigation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        stats.TakeDamage(amount);
    }

    public void Spawn()
    {
        GetComponent<EnemySpawning>().Spawn();
    }



}


