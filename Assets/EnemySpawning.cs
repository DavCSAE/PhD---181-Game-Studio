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
    }

    public void Spawned()
    {
        
        isSpawning = false;
        TurnCollidersOn();
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
