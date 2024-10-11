using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    // Components and References
    public Rigidbody2D enemyRigidBody;   // Reference to the player's Rigidbody2D for physics and movement.
    public GameObject stage;             // Reference to the stage GameObject (for ground checks).
    private SpriteRenderer spriteRenderer; // SpriteRenderer for changing player sprites.
    public AudioSource deathSound;       // A sound that gets played when the character gets destroyed

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
