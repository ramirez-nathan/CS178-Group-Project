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

    // Sprites
    public Sprite attack;                // Sprite for the attack action.
    private Sprite defaultSprite;        // Stores the default sprite to revert after an attack.
    public bool isFacingRight = true;    // Tracks whether the player's sprite is facing right
    public Animator animator;            // Controls all the animations of the player.

    // Jumping/Movement Mechanics
    private bool isOnStage = true;       // Tracks if the player is on the stage (grounded).
    private int jumpCount = 0;           // Tracks the number of jumps performed.
    private const int maxJumps = 2;      // Maximum number of allowed jumps (double jump).
    public float jumpForce = 12f;        // Jump force applied to the player when jumping.

    public float moveSpeedX = 10f; // X Movement Speed of Player
    public float moveSpeedY = 1f; // Y Movement Speed of Player
    private Vector2 currentVelocity; // Direction of Player

    // Input System 
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction neutralGAttack;
    private InputAction dashGAttack;


    // Combat and Health
    public int health = 100;             // Player's health points.
    public float attackDuration = 0.3f;  // Duration (in seconds) the attack sprite stays visible before reverting.
    public float attackDamage = 0;       // Amount of damage done by an attack

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
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get and store the SpriteRenderer component attached to this GameObject.
        defaultSprite = spriteRenderer.sprite;            // Store the initial sprite from the SpriteRenderer as the default sprite.
        animator = GetComponent<Animator>();              // Initializing the animator
    }


    // Update is called once per frame - Process inputs here
    void Update()
    {
        //ProcessInputs();
        // Store the current velocity to avoid overwriting it
        currentVelocity = playerRigidBody.velocity;

        currentVelocity.x = move.ReadValue<Vector2>().x * 10f; 

        //if (currentVelocity.x > 0 || currentVelocity.x < 0)
        //{
        //    //Debug.Log(move.ReadValue<Vector2>().x * 0.2f);
        //    //Debug.Log("Moving");   
        //}

        UpdateSpriteDirection();

        // Jumping
        if (jump.WasPressedThisFrame() /*&& jump.phase == InputActionPhase.Started*/) 
        {
            // First jump only allowed if on the stage, Allow a second jump while airborne (double jump)
            if (jumpCount == 0 || jumpCount == 1)
            {
                currentVelocity.y = jumpForce; // Apply upward velocity for first jump
                jumpCount++;
            
                animator.SetBool("isJumping", !isOnStage); // Lets the animator know that the player is now jumping
            }
        }
        // When jump key is released, set vert speed to 20% (Jump Cutting)
        if (jump.WasReleasedThisFrame() && currentVelocity.y > 0)
        {
            currentVelocity.y = currentVelocity.y * 0.20f;
        }

        CheckForAttack();


        animator.SetBool("isJumping", !isOnStage); // Lets the animator know that the player is now jumping
    }

    // Fixed Update is called a set amount of times - Do physics here
    private void FixedUpdate()
    {
        // Apply the velocity back to the Rigidbody2D
        playerRigidBody.velocity = currentVelocity;

        animator.SetFloat("xVelocity", Mathf.Abs(playerRigidBody.velocity.x));
        animator.SetFloat("yVelocity", playerRigidBody.velocity.y);

        // Checks to see if player is out of bounds and destroys player if true
        if (transform.position.x > outOfBoundsXRight || transform.position.x < outOfBoundsXLeft || transform.position.y < outOfBoundsY)
        {
            Debug.Log("You have been destroyed");
            PlayDeathSound();
        }
    }

    void CheckForAttack()
    {
        if (dashGAttack.triggered)
        {
            Debug.Log("DashGAttack performed"); 
        }
        else if (neutralGAttack.triggered && !move.WasPressedThisFrame())
        {
            Debug.Log("NeutralGAttack performed");
            StartCoroutine(PerformAttack(0));
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
        // animator.SetInteger("attackType", attackNum);
        // Change to the attack sprite
        spriteRenderer.sprite = attack;

        // Wait for the duration of the attack
        yield return new WaitForSeconds(attackDuration);

        //Debug.Log("Swing");

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

            // Immediately destroy the GameObject
            Destroy(gameObject);
        }
    }



    void OnCollisionEnter2D(Collision2D collision)      // Checks if player is on the stage
    {
        if (collision.gameObject == stage)
        {
            isOnStage = true;
            jumpCount = 0;
            Debug.Log("Collision Happened, jumpCount is " + jumpCount);
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
