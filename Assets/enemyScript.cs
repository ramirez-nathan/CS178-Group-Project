using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    // Components and References
    public Rigidbody2D enemyRigidBody;     // Reference to the player's Rigidbody2D for physics and movement.
    public GameObject player;              // Reference to the player object
    private SpriteRenderer spriteRenderer; // SpriteRenderer for changing player sprites.
    public AudioSource deathSound;         // A sound that gets played when the character gets destroyed

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
    private float currentHealth = 0;
    public float attackDuration = 0.3f;  // Duration the attack sprite stays visible before reverting.
    public float attackDamage = 0;       // Amount of damage done by an attack
    public float speed = 0.3f;           // How fast the enemy moves
    public float knockBack = 0;          // How far an attack will knock back someone

    // Out of bounds range, x = +- 11, y = -7
    private float outOfBoundsXLeft = -11f;
    private float outOfBoundsXRight = 11f;
    private float outOfBoundsY = -7f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player exists
        if (player != null)
        {
            // Movement for chasing the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            // Debug message for when player is dead
            Debug.Log("Player has been destroyed, stopping chase.");
        }

        // Checks to see if the enemy is out of bounds and destroys enemy if true
        if (transform.position.x > outOfBoundsXRight || transform.position.x < outOfBoundsXLeft || transform.position.y < outOfBoundsY)
        {
            Debug.Log("Enemy has been destroyed");
            PlayDeathSound();
        }
    }

    void PlayDeathSound()
    {
        if (deathSound != null && deathSound.clip != null)
        {
            // Play the sound at the character's position
            AudioSource.PlayClipAtPoint(deathSound.clip, transform.position);

            // Set hp equal to 0
            health = 0;

            // Immediately destroy the GameObject
            Destroy(gameObject);
        }
    }

    // Attacks
    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        // Reduce health
        currentHealth -= damage;

        // Apply knockback
        Knockback(knockbackDirection, knockbackForce);

        // Check if health is less than or equal to 0
        if (currentHealth <= 0)
        {
            PlayDeathSound();  // Trigger death
        }
    }
    private void Knockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }


}
