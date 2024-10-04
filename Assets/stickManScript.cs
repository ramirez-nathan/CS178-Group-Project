using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickManScript : MonoBehaviour
{
    public Rigidbody2D stickRigidBody;

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
            currentVelocity.y = 12; // Apply upward velocity when space is pressed
        }

        // Apply the velocity back to the Rigidbody2D
        stickRigidBody.velocity = currentVelocity;
    }
}
