using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    // PLAYER COMPONENTS
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerAnimations animations;
    [HideInInspector]
    public PlayerSpawning spawning;
    [HideInInspector]
    public PlayerCollision collision;
    [HideInInspector]
    public PlayerAppearance appearance;
    [HideInInspector]
    public PlayerReceivableItems receivables;
    [HideInInspector]
    public PlayerCombat combat;
    [HideInInspector]
    public PlayerTargeting targeting;
    [HideInInspector]
    public PlayerCameras cameras;

    // PHYSICS
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public CapsuleCollider capsColl;

    // Unlockables
    [HideInInspector]
    public bool isMaskUnlocked;
    //[HideInInspector]
    public bool isSwordUnlocked;
    [HideInInspector]
    public bool isDashUnlocked;
    [HideInInspector]
    public bool areWingsUnlocked;

    // Respawning

    void Awake()
    {
        Singleton = this;

        // PLAYER COMPONENTS
        movement = GetComponent<PlayerMovement>();
        animations = GetComponent<PlayerAnimations>();
        spawning = GetComponent<PlayerSpawning>();
        collision = GetComponent<PlayerCollision>();
        appearance = GetComponent<PlayerAppearance>();
        receivables = GetComponent<PlayerReceivableItems>();
        combat = GetComponent<PlayerCombat>();
        stats = GetComponent<PlayerStats>();
        targeting = GetComponent<PlayerTargeting>();
        cameras = GetComponent<PlayerCameras>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // PHYSICS
        rb = GetComponent<Rigidbody>();
        capsColl = GetComponent<CapsuleCollider>();

        // INPUTS
        InputManager.Singleton.EnableInputs();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerFell();

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            SoundManager.Singleton.Play("Item sound");
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            LevelLoader.Singleton.LoadLevel(2);
        }
    }

    void CheckIfPlayerFell()
    {
        if (transform.position.y < -20f)
        {
            spawning.FadeForSpawn();
        }
    }

    public void UnParent()
    {
        transform.parent = null;
    }

    public void UnlockMask()
    {
        
        GameManager.Singleton.UnlockMask();

        appearance.ShowMask();
    }

    public void UnlockSword()
    {
        GameManager.Singleton.UnlockSword();

        combat.UnlockSword();
    }

    public void UnlockDash()
    {
        GameManager.Singleton.UnlockDash();

        movement.isDashUnlocked = true;
    }

    public void UnlockWings()
    {
        GameManager.Singleton.UnlockWings();

        movement.doubleJumpEnabled = true;
        appearance.ShowWings();
    }
    
}
