using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    bool isUnlocked;

    public GroundPortal GetPortal()
    {
        return portal;
    }

    public void UnlockCheckpoint()
    {
        if (isUnlocked) return;

        isUnlocked = true;

        SetActiveCheckpoint();
    }

    void SetActiveCheckpoint()
    {
        PlayerSpawnPoint spawnPoint = GetComponentInChildren<PlayerSpawnPoint>();
        Player.Singleton.spawning.SetTargetSpawnPoint(spawnPoint);
    }
}
