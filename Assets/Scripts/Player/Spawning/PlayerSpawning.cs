using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerSpawning : MonoBehaviour
{
    Player player;
    [SerializeField] PlayerSpawnPoint targetSpawnPoint;

    bool isPreparingToSpawn;
    bool isSpawning;

    // Player's camera controller
    CinemachineFreeLook cmFreeLook;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        cmFreeLook = GetComponentInChildren<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            FadeForSpawn();
        }
    }

    public void FadeForSpawn()
    {
        BlackScreen.Singleton.FadeToBlack(PrepareToSpawn);
    }

    public void PrepareToSpawn()
    {

        // Set player position to spawn point position
        transform.position = targetSpawnPoint.transform.position;

        // Update state so PlayerAnimations knows what to do
        isPreparingToSpawn = true;

        // Freeze Player movement
        player.movement.Freeze();

        // Fade from the black screen
        BlackScreen.Singleton.FadeFromBlack();

        // Position the camera based on spawn point data
        PositionCamera();

        // Rotate the player based on spawn point data
        SetPlayerRotation();


        // Open the spawn portal after the black screen is gone
        Invoke("OpenPortal", 1f);

        // Spawn the player after the portal is opened
        Invoke("Spawn", 1.2f);
    }

    void OpenPortal()
    {
        targetSpawnPoint.OpenPortal();
    }

    void Spawn()
    {
        isPreparingToSpawn = false;

        isSpawning = true;

    }

    public void Spawned()
    {
        isSpawning = false;

        player.movement.Unfreeze();
    }

    public bool GetSpawningState()
    {
        return isSpawning;
    }

    public bool GetPreparingToSpawnState()
    {
        return isPreparingToSpawn;
    }

    public void SetTargetSpawnPoint(PlayerSpawnPoint sp)
    {
        targetSpawnPoint = sp;
    }

    public void StayPreparedToSpawn()
    {
        isPreparingToSpawn = true;

        // Freeze Player movement
        player.movement.Freeze();

        PositionCamera();
        SetPlayerRotation();
    }

    void PositionCamera()
    {
        cmFreeLook.m_XAxis.Value = targetSpawnPoint.GetCamXAxis();
        cmFreeLook.m_YAxis.Value = targetSpawnPoint.GetCamYAxis();
    }

    void SetPlayerRotation()
    {
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x, // Keep x rotation
            targetSpawnPoint.GetPlayerYRot(), // New y rotation
            transform.localEulerAngles.z); // Keep z rotation
    }
}
