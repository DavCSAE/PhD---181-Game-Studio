using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{

    [SerializeField] EnemySpawnPoint targetSpawnPoint;

    public bool isPreparingToSpawn;
    public bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        if (isPreparingToSpawn) TurnCollidersOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        isSpawning = true;
        isPreparingToSpawn = false;
        targetSpawnPoint.OpenPortal();
        GetComponent<EnemyCombat>().ActivateSword();

        //SoundManager.Singleton.Stop("Opening Intro");
        //SoundManager.Singleton.Play("Battle");
    }

    public void Spawned()
    {
        
        isSpawning = false;
        TurnCollidersOn();

        GetComponent<EnemyCombat>().StartAttacking();
    }

    void TurnCollidersOff()
    {
        GetComponent<ShadeIdleAnimations>().DeactivateColldiers();
    }

    void TurnCollidersOn()
    {
        GetComponent<ShadeIdleAnimations>().ActivateColliders();
    }
}
