using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PLAYER COMPONENTS
    //[HideInInspector]
    public PlayerMovement movement;
    //[HideInInspector]
    public PlayerAnimations animations;
    [HideInInspector]
    public PlayerCollision collision;

    // PHYSICS
    //[HideInInspector]
    public Rigidbody rb;
    //[HideInInspector]
    public CapsuleCollider capsColl;

    // CAMERA
    [HideInInspector]
    public Camera cam;

    void Awake()
    {
        
    }



    // Start is called before the first frame update
    void Start()
    {
        

        // PLAYER COMPONENTS
        movement = GetComponent<PlayerMovement>();
        movement.player = this;
        animations = GetComponent<PlayerAnimations>();
        animations.player = this;
        collision = GetComponent<PlayerCollision>();
        collision.player = this;

        // PHYSICS
        rb = GetComponent<Rigidbody>();
        capsColl = GetComponent<CapsuleCollider>();

        // CAMERA
        cam = Camera.main;


        // INPUTS
        InputManager.Singleton.EnableInputs();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerFell();
    }

    void CheckIfPlayerFell()
    {
        if (transform.position.y < -20f)
        {
            Vector3 respawnPos = new Vector3(0, 2, 0);
            transform.position = respawnPos;

            print("respawn!");
        }
    }
}
