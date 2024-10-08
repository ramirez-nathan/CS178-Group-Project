using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickManScript : MonoBehaviour
{
    // Components and References
    public Rigidbody2D stickRigidBody;   // Reference to the player's Rigidbody2D for physics and movement.
    public GameObject stage;             // Reference to the stage GameObject (for ground checks).
    private SpriteRenderer spriteRenderer; // SpriteRenderer for changing player sprites.

    // Sprites
    public Sprite attack;                // Sprite for the attack action.
    private Sprite defaultSprite;        // Stores the default sprite to revert after an attack.

    // Jumping Mechanics
    private bool isOnStage = true;       // Tracks if the player is on the stage (grounded).
    private int jumpCount = 0;           // Tracks the number of jumps performed.
    private const int maxJumps = 2;      // Maximum number of allowed jumps (double jump).
    public float jumpForce = 12f;        // Jump force applied to the player when jumping.

    // Combat and Health
    public int health = 100;             // Player's health points.
    public float attackDuration = 0.3f;  // Duration the attack sprite stays visible before reverting.
    public float attackDamage = 0;       // Amount of damage done by an attack

    // Out of bounds range, x = +- 11, y = -7
    private float outOfBoundsXLeft = -11f;
    private float outOfBoundsXRight = 11f;
    private float outOfBoundsY = -7f;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get and store the SpriteRenderer component attached to this GameObject.
        defaultSprite = spriteRenderer.sprite;            // Store the initial sprite from the SpriteRenderer as the default sprite.
        gameObject.name = "stickManFighter";              // Rename the GameObject to "stickManFighter" for better identification in the hierarchy.
    }


    // Update is called once per frame
    void Update()
    {
        // Store the current velocity to avoid overwriting it
        Vector2 currentVelocity = stickRigidBody.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            currentVelocity.x = -10; // Moves left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentVelocity.x = 10; // Moves right
        }
        else
        {
            currentVelocity.x = 0; // Stops horizontal movement when no keys are pressed
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // First jump only allowed if on the stage, Allow a second jump while airborne (double jump)
            if (jumpCount == 0 || jumpCount == 1)
            {
                currentVelocity.y = jumpForce; // Apply upward velocity for first jump
                jumpCount++;
            }
  
    }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PerformAttack());
        }


        // Apply the velocity back to the Rigidbody2D
        stickRigidBody.velocity = currentVelocity;

        if (transform.position.x > outOfBoundsXRight  || transform.position.x  < outOfBoundsXLeft || transform.position.y < outOfBoundsY)
        {
            Debug.Log("You have been destroyed");
            Destroy(gameObject);
        }
    }

    // Coroutine to handle the attack animation and revert to idle
    private IEnumerator PerformAttack()
    {
        // Change to the attack sprite
        spriteRenderer.sprite = attack;

        // Wait for the duration of the attack
        yield return new WaitForSeconds(attackDuration);

        Debug.Log("Swing");

        // Revert to the idle sprite
        spriteRenderer.sprite = defaultSprite;
    }

    //private void Jump()
    //{
    //    stickRigidBody.velocity = new Vector2(stickRigidBody.velocity.x, jumpForce);
    //    jumpCount++; 
    //}

    void OnCollisionEnter2D(Collision2D collision)      // Checks if player is on the stage
    {
        if (collision.gameObject == stage)
        {
            isOnStage = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)       // Sets on stage to false when player leaves the stage
    {
        if (collision.gameObject == stage)
        {
            isOnStage = false;
        }
    }
}
