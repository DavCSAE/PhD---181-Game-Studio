using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        Enemy[] tempEnemies = GetComponentsInChildren<Enemy>();

        foreach (Enemy enemy in tempEnemies)
        {
            enemies.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            //print("SPAWN ENEMIES : " + enemies.Count);
            //SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Spawn();
        }
    }
}
