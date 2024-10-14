using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Components and References
    public Rigidbody2D playerRigidBody;   // Reference to the player's Rigidbody2D for physics and movement.
    public GameObject stage;             // Reference to the stage GameObject (for ground checks).
    private SpriteRenderer spriteRenderer; // SpriteRenderer for changing player sprites.
    public AudioSource deathSound;       // A sound that gets played when the character gets destroyed
    public Collider2D attackCollider;    // The collider representing the player's attack hitbox
    public enemyScript enemyScwipt;      // Reference to enemy code

    // Sprites
    public Sprite attack;                // Sprite for the attack action.
    private Sprite defaultSprite;        // Stores the default sprite to revert after an attack.
    public bool isFacingRight = true;    // Tracks whether the player's sprite is facing right
    public Animator animator;            // Controls all the animations of the player.

    // Jumping/Movement Mechanics
    private bool isOnFloor = true;       // Tracks if the player is on the stage (grounded).
    protected bool jumpPressed = false;
    protected bool jumpReleased = false;
    private int jumpCount = 0;           // Tracks the number of jumps performed.
    private const int maxJumps = 2;      // Maximum number of allowed jumps (double jump).
    public float jumpForce = 12f;        // Jump force applied to the player when jumping.

    public float moveSpeedX = 10f; // X Movement Speed of Player
    public float moveSpeedY = 1f; // Y Movement Speed of Player
    private Vector2 currentVelocity; // Current velocity of Player

    // Input System 
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction neutralGAttack;
    private InputAction dashGAttack;


    // Combat and Health
    public int health = 100;             // Player's health points.
    public float currentHealth = 0;      // The current Players health points
    public float attackDuration = 0.3f;  // Duration (in seconds) the attack sprite stays visible before reverting.
    public int attackDamage = 10;       // Amount of damage done by an attack
    public float knockBack = 1f;          // How far an attack will knock back someone

    // Out of bounds range, x = +- 11, y = -7
    private float outOfBoundsXLeft = -11f;
    private float outOfBoundsXRight = 11f;
    private float outOfBoundsY = -7f;

    // Awake is called when the script loads
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        dashGAttack = playerControls.Player.DashGAttack;
        dashGAttack.Enable();

        neutralGAttack = playerControls.Player.NeutralGAttack;
        neutralGAttack.Enable();

        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        dashGAttack.Disable();

        neutralGAttack.Disable();

        move.Disable();

        jump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get and store the SpriteRenderer component attached to this GameObject.
        defaultSprite = spriteRenderer.sprite;            // Store the initial sprite from the SpriteRenderer as the default sprite.
        animator = GetComponent<Animator>();              // Initializing the animator
        attackCollider.enabled = false;
    }


    // Update is called once per frame - Process inputs here
    void Update()
    {
        currentVelocity = playerRigidBody.velocity; // Store the current velocity to avoid choppy movement

        currentVelocity.x = move.ReadValue<Vector2>().x * 10f; 

        UpdateSpriteDirection();

        ProcessInputs();

        CheckForAttack();

        animator.SetBool("isJumping", !isOnFloor); // animator checks if player is jumping still
    }

    // Fixed Update is called a set amount of times - Do physics here
    private void FixedUpdate()
    {
        HandleJump();
        // Apply the velocity back to the Rigidbody2D
        animator.SetFloat("xVelocity", Mathf.Abs(playerRigidBody.velocity.x));
        animator.SetFloat("yVelocity", playerRigidBody.velocity.y);

        playerRigidBody.velocity = currentVelocity;
        // Checks to see if player is out of bounds and destroys player if true
        if (transform.position.x > outOfBoundsXRight || transform.position.x < outOfBoundsXLeft || transform.position.y < outOfBoundsY)
        {
            Debug.Log("You have been destroyed");
            KillPlayer();
        }
        
    }

    void ProcessInputs()
    {
        // Jumping
        if (jump.WasPressedThisFrame())
        {
            // checks the jump count to see if player has jumps left (double jump)
            if (jumpCount == 0 || jumpCount == 1)
            {
                jumpPressed = true;
                animator.SetBool("isJumping", !isOnFloor); // Lets the animator know that the player is now jumping
            }
        }
        // When jump key is released, set vert speed to 20% (Jump Cutting)
        if (jump.WasReleasedThisFrame() && currentVelocity.y > 0)
        {
            jumpReleased = true;
        }
        else
        {
            Debug.Log(jumpCount);
        }
    }
    
    void HandleJump()
    {
        if (jumpPressed)
        {           
            jumpCount++;
            currentVelocity.y = jumpForce; // Apply upward velocity for first jump
            jumpPressed = false; 
        }
        if (jumpReleased && currentVelocity.y > 0)
        {
            currentVelocity.y *= 0.20f;
            jumpReleased = false;
        }
    }


    // HANDLE ATTACKS 
    void CheckForAttack()
    {
        if (dashGAttack.triggered)
        {
            Debug.Log("DashGAttack performed"); 
        }
        else if (neutralGAttack.triggered && !move.WasPressedThisFrame())
        {
            Debug.Log("NeutralGAttack performed");
            StartCoroutine(PerformAttack(1));
        }
    }

    // ADJUST WHICH WAY SPRITE IS FACING
    void UpdateSpriteDirection() 
    {
        if(isFacingRight && playerRigidBody.velocity.x < 0f || !isFacingRight && playerRigidBody.velocity.x > 0f) 
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }


    // Coroutine to handle the attack animation and revert to idle
    private IEnumerator PerformAttack(int attackNum)
    { 
        animator.SetInteger("attackType", attackNum);

        // Change to the attack sprite
        spriteRenderer.sprite = attack;

        Debug.Log("attack enabled");
        attackCollider.enabled = true;
        // Wait for the duration of the attack
        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false;

        Debug.Log("Swing");

        animator.SetInteger("attackType", 0);

        // Revert to the idle sprite
        spriteRenderer.sprite = defaultSprite;
    }

    // DESTROYS PLAYER OBJECT & PLAYS DEATH SOUND
    void KillPlayer()
    {
        if (deathSound != null && deathSound.clip != null)
        {
            // Play the sound at the character's position
            AudioSource.PlayClipAtPoint(deathSound.clip, transform.position);

            // Set hp equal to 0
            health = 0;
            currentHealth = 0;

            // Immediately destroy the GameObject
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)      // Checks if player is on the stage
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TopStage")) // checking top stage layer 
        {
            isOnFloor = true;
            jumpCount = 0;
            //Debug.Log("Collision Happened, jumpCount is " + jumpCount);
        }
    }

    void OnCollisionExit2D(Collision2D collision)       // Sets on stage to false when player leaves the stage
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TopStage")) // checking top stage layer 
        {
           isOnFloor = false;
        }
    }

    //Attacks
    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        // Reduce health
        currentHealth -= damage;

        // Apply knockback
        Knockback(knockbackDirection, knockbackForce);

        // Check if health is less than or equal to 0
        if (currentHealth <= 0)
        {
            KillPlayer();  // Trigger death
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we collided with has an EnemyScript component
        enemyScwipt = collision.GetComponent<enemyScript>();

        if (enemyScwipt != null)
        {
            // Calculate the direction for knockback (from the player to the enemy)
            Vector2 knockbackDirection = (enemyScwipt.transform.position - transform.position).normalized;

            // Apply damage and knockback to the enemy
            enemyScwipt.TakeDamage(attackDamage, knockbackDirection, knockBack);
        }
        else
        {
            // Optional: Debug to see what other object we might have collided with
            Debug.Log("Collision with non-enemy object: " + collision.name);
        }
    }



}
