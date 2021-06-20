using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    [Header("STATES")]
    // Movement states
    public bool isMoving;
    public bool isGrounded;
    public bool isJumping;
    public bool isFalling;
    public bool isDashing;

    // Inputs
    Vector2 movementInput;
    bool jumpInput;

    // Stops all movement if true
    bool isFrozen = false;

    // GROUNDING
    float footSnapDist = 0.01f;
    
    [Header("MOVEMENT")]
    // Horizontal movement
    [SerializeField]
    float moveSpeed = 3f;
    Vector3 moveDirection;

    // Rotation
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Header("JUMPING")]
    // Jumping / Falling
    [SerializeField]
    float jumpHeight = 3f;
    Vector3 velocityBeforeJump;
    [SerializeField] float loseVelocityInAirSpeed = 3f;
    float fallMultiplier = 2.5f;
    float lowJumpModifier = 2f;
    bool doubleJumpEnabled;
    bool canDoubleJump;
    bool unlimitedJumpsEnabled;
    float jumpBoost = 5f;

    [Header("DASHING")]
    // Dashing
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashLength = 3f;
    Vector3 dashDir;
    Vector3 dashStartPos;

    [Header("MOVING PLATFORMS")]
    // Platform movement
    public bool onPlatform;

    private void OnEnable()
    {
        PlayerEvents.JumpEvent += Jump;
        PlayerEvents.DashEvent += Dash;
    }

    private void OnDisable()
    {
        PlayerEvents.JumpEvent -= Jump;
        PlayerEvents.DashEvent -= Dash;
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Don't do anything if frozen
        if (isFrozen) { return; }

        GetMovementInputs();

        HandleGrounding();
        HandleMovement();
        HandleRotation();
        HandleJumping();
        HandleDashing();
    }

    void GetMovementInputs()
    {
            // Get movement input
            movementInput = InputManager.Singleton.GetMoveInput();

            // Get jump input
            jumpInput = InputManager.Singleton.GetJumpInput();
    }

    void HandleGrounding()
    {
        // Don't do anything if starting jump
        // Prevents player from being stuck on ground when trying to jump
        if (isJumping) { return; }

        // I set layer 8 as the 'player' layer in unity
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask; // Everything but player layer

        // Calculate position of bottom of collider to use as origin of raycast
        Vector3 botOfColl = transform.position + player.capsColl.center + Vector3.down * player.capsColl.height / 2;

        // Calculate how far to shoot ray
        float rayDistance = botOfColl.y - transform.position.y + footSnapDist;

        // RaycastHit to store hit info
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        // Checks if the player is on the ground

        if (Physics.SphereCast(botOfColl, player.capsColl.radius,
            Vector3.down, out hit, rayDistance, layerMask))
        {
            Debug.DrawRay(botOfColl, Vector3.down * rayDistance, Color.red);

            // Player is on the ground
            isGrounded = true;
            isFalling = false;
        }
        else // In air
        {
            Debug.DrawRay(botOfColl, Vector3.down * rayDistance, Color.blue);
            Debug.DrawRay(transform.position + player.capsColl.center + Vector3.down * player.capsColl.height / 2, Vector3.down * rayDistance, Color.green);

            // If stepped off platform or ledge and is now falling
            if (isGrounded)
            {
                // Keep some velocity of the movement from before falling
                velocityBeforeJump = player.rb.velocity / 2;
            }

            // Update states
            isGrounded = false;
            isFalling = true;

            // Activate gravity for player
            player.rb.useGravity = true;

            // (FOR BETTER JUMPING FEEL)
            // If falling
            if (player.rb.velocity.y < 0)
            {
                // Make player fall faster
                player.rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }

            /*
            // If wings have flapped
            if (player.animations.GetFlapStatus())
            {
                // Return wings to original position
                player.animations.Flapped();
            }
            */
            print("in air");

            // Release Plaftorm
            if (transform.parent != null)
            {
                transform.parent = null;

                //onPlatform = false;

                print("off PLatform!");
            }

            if (onPlatform)
            {
                onPlatform = false;

                transform.parent = null;

                transform.localScale = Vector3.one;

            }
            
        }

        // If player is now on ground
        if (isGrounded)
        {
            // Turn off gravity
            player.rb.useGravity = false;

            // Set target y position to position the raycast hit
            float yPosTarget = hit.point.y;

            // Create a target position based off the players current position and the 'y target position'
            Vector3 targetPosition = transform.position;
            targetPosition.y = yPosTarget;

            // Set player's position to target position
            transform.position = targetPosition;

            // Reset velocity before jump, since player has landed
            velocityBeforeJump = Vector3.zero;

            // Remove any 'y' velocity from player's velocity
            player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);

            // Is the hit object a moving platform?
            bool hitMovingPlatform = (hit.collider.gameObject.layer == LayerMask.NameToLayer("Moving Platform"));

            // If landed on platform
            if (hitMovingPlatform && !onPlatform)
            {
                // Set player's parent object to the platform that they landed on
                transform.parent = hit.collider.transform.parent;

                print("onPlatform!");

                onPlatform = true;
            }
            else if (!hitMovingPlatform && onPlatform)
            {
                onPlatform = false;
                transform.parent = null;
            }
            
        }
    }

    void HandleMovement()
    {
        // Create normalized direction vector out of movement inputs
        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        // Move Character if getting any move inputs
        if (direction.magnitude > 0f)
        {
            // Get the angle that the input direction needs to be rotated so that direction is based off camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + player.cam.transform.eulerAngles.y;

            //Set the move direction based on the camera
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Normalize move direction
            Vector3 movDir = moveDirection.normalized;

            // Get movement speed
            float speed = moveSpeed * Time.deltaTime * 100;

            // If in the air
            if (!isGrounded)
            {
                // Reduce speed
                speed *= 0.25f;
            }

            // Calculate players new velocity
            Vector3 newVelocity = new Vector3(moveDirection.normalized.x * speed + velocityBeforeJump.x,
                player.rb.velocity.y, moveDirection.normalized.z * speed + velocityBeforeJump.z);

            // Set players new velocity
            player.rb.velocity = newVelocity;

            // Update 'isMoving' state based off current velocity
            if (player.rb.velocity != Vector3.zero)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask; // Everything but player layer

            // RaycastHit to store hit info
            RaycastHit hit;
            // Stops player from getting too close to walls (Player will climb them if too close)
            // Front
            if (Physics.Raycast(transform.position + Vector3.up / 2, transform.forward, player.capsColl.radius + 0.2f, layerMask))
            {
                player.rb.velocity -= Vector3.Project(player.rb.velocity, transform.forward);
            }
            // Front-right
            if (Physics.Raycast(transform.position + Vector3.up / 2, transform.forward + transform.right, player.capsColl.radius + 0.2f, layerMask))
            {
                player.rb.velocity -= Vector3.Project(player.rb.velocity, transform.forward + transform.right);
            }
            // Front-left
            if (Physics.Raycast(transform.position + Vector3.up / 2, transform.forward + -transform.right, player.capsColl.radius + 0.2f, layerMask))
            {
                player.rb.velocity -= Vector3.Project(player.rb.velocity, transform.forward + -transform.right);
            }
            // Right
            if (Physics.Raycast(transform.position + Vector3.up / 2, transform.right, player.capsColl.radius + 0.2f, layerMask))
            {
                player.rb.velocity -= Vector3.Project(player.rb.velocity, transform.right);
            }
            // Left
            if (Physics.Raycast(transform.position + Vector3.up / 2, -transform.right, player.capsColl.radius + 0.2f, layerMask))
            {
                player.rb.velocity -= Vector3.Project(player.rb.velocity, -transform.right);
            }
        }
        // If there is no movement input and player is on the ground
        else if (isGrounded)
        {
            // Remove all velocity from player
            player.rb.velocity = Vector3.zero;

            // Reset moveDirection
            moveDirection = Vector3.zero;

            // Update isMoving state
            isMoving = false;
        }
        // If there is no movement input and player is in the air
        else if (!isGrounded)
        {
            // Reset moveDirection
            moveDirection = Vector3.zero;

            // Calculate player's velocity based on the velocity before jumping and the current 'y' velocity (from gravity or jumping)
            Vector3 newVelocity = new Vector3(velocityBeforeJump.x,
            player.rb.velocity.y, velocityBeforeJump.z);

            // Set the player's velocity
            player.rb.velocity = newVelocity;
        }
    }

    void HandleRotation()
    {
        // Create normalized direction vector out of movement inputs
        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (direction.magnitude > 0f)
        {
            // Get the angle that the input direction needs to be rotated so that direction is based off camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
            + player.cam.transform.eulerAngles.y;

            // Get smoothed out angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity, turnSmoothTime);

            // Rotate the player by the smoothed out angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void HandleJumping()
    {
        // If jumping
        if (isJumping)
        {
            // (FOR BETTER JUMPING FEEL)
            // If player is rising upwards AND player isn't holding jump button
            if (player.rb.velocity.y > 0 && !InputManager.Singleton.GetJumpInput())
            {
                // Make player not rise as much / fall faster
                player.rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpModifier - 1) * Time.deltaTime;
            }

            // If player is falling
            if (player.rb.velocity.y < 0)
            {
                // Update movement states
                isJumping = false;
                isFalling = true;
            }
        }

        // If player is in the air and still has horizontal velocity from before being in the air
        if (!isGrounded && velocityBeforeJump != Vector3.zero)
        {
            // Lessen the amount of horizontal velocity
            velocityBeforeJump.x = Mathf.Lerp(velocityBeforeJump.x, 0, loseVelocityInAirSpeed * Time.deltaTime);
            velocityBeforeJump.z = Mathf.Lerp(velocityBeforeJump.z, 0, loseVelocityInAirSpeed * Time.deltaTime);

            // If horizontal velocity is close to 0, set it to 0
            if (velocityBeforeJump.x < 0.1 && velocityBeforeJump.x > -0.1)
                velocityBeforeJump.x = 0;
            if (velocityBeforeJump.z < 0.1 && velocityBeforeJump.z > -0.1)
                velocityBeforeJump.z = 0;
        }
    }

    public void Jump()
    {
        print("JUMP!");

        // If player is on the ground OR in the air and can double jump
        if (isGrounded || canDoubleJump)
        {
            // If player is in the air AND can double jump
            if (!isGrounded && canDoubleJump)
            {
                // Stop player from falling
                player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);

                // If player can't jump unlimited times
                if (!unlimitedJumpsEnabled)
                {
                    // Prevent from jumping again after double jump
                    canDoubleJump = false;
                }

                // Play 'Wing Flap' animation
                //player.animations.FlapWings();

                // Give horizontal boost in movement direction
                player.rb.velocity = new Vector3(player.rb.velocity.x + moveDirection.normalized.x * jumpBoost,
                    player.rb.velocity.y, player.rb.velocity.z + moveDirection.z * jumpBoost);
            }
            else
            {
                // If double jump is enabled (Player has wings)
                if (doubleJumpEnabled)
                {
                    // Allow player to double jump (In certain conditions)
                    canDoubleJump = true;
                }
            }

            // Update states
            isJumping = true;
            isGrounded = false;

            // Store velocity before jump to player's velocity at time of starting jump
            velocityBeforeJump = player.rb.velocity;

            // Turn player's gravity on
            player.rb.useGravity = true;

            // Create jump vector
            Vector3 jumpVector = new Vector3(0, jumpHeight, 0);

            // Add the jump vector as a force to the player
            // Add the player's current velocity as well to help player jump in direction they are moving
            player.rb.AddForce(player.rb.velocity + jumpVector * 100);
        }


        // If player is on a moving platform (Has the platform as their parent object)
        if (transform.parent != null)
        {
            // Remove platform from being player's parent object
            transform.parent = null;
        }

        if (onPlatform)
        {
            onPlatform = false;

            transform.localScale = Vector3.one;
        }
    }

    void EnableDoubleJump()
    {
        doubleJumpEnabled = true;
    }

    public void ToggleUnlimitedJumps()
    {
        unlimitedJumpsEnabled = !unlimitedJumpsEnabled;
    }

    void HandleDashing()
    {
        if (isDashing)
        {
            player.rb.velocity += dashDir * dashSpeed;

            if (Vector3.Distance(dashStartPos, transform.position) >= dashLength)
            {
                if (isGrounded)
                {
                    player.rb.velocity = Vector3.zero;
                }
                else
                {
                    velocityBeforeJump = player.rb.velocity;
                }
                isDashing = false;
            }
        }
    }

    void Dash()
    {
        print("DASH!");

        dashStartPos = transform.position;

        isDashing = true;

        dashDir = moveDirection.normalized;

        //player.rb.AddForce(moveDirection * 100 * dashPower);
    }

    public void Unfreeze()
    {
        isFrozen = false;

        InputManager.Singleton.EnableInputs();
    }

    public void Freeze()
    {
        isFrozen = true;

        player.rb.useGravity = false;
        player.rb.velocity = Vector3.zero;

        InputManager.Singleton.DisableInputs();
    }
}
