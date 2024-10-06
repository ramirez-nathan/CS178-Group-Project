using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickManScript : MonoBehaviour
{
    public Rigidbody2D stickRigidBody;
    public GameObject stage;
    public Sprite attack;
    private SpriteRenderer spriteRenderer;
    private bool isOnStage = true;
    private int jumpCount = 0;         
    private const int maxJumps = 2;
    public float jumpForce = 12f;
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "stickManFighter";
    }

    // Update is called once per frame
    void Update()
    {
        // Store the current velocity to avoid overwriting it
        Vector2 currentVelocity = stickRigidBody.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            currentVelocity.x = -10; // Move left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentVelocity.x = 10; // Move right
        }
        else
        {
            currentVelocity.x = 0; // Stop horizontal movement when no keys are pressed
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // First jump only allowed if on the stage
            if (isOnStage && jumpCount == 0)
            {
                currentVelocity.y = jumpForce; // Apply upward velocity for first jump
                jumpCount++;
            }
            // Allow a second jump while airborne (double jump)
            else if (jumpCount == 1)
            {
                currentVelocity.y = jumpForce; // Apply upward velocity for second jump
                jumpCount++;
            }
  
    }

        // Apply the velocity back to the Rigidbody2D
        stickRigidBody.velocity = currentVelocity;
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
