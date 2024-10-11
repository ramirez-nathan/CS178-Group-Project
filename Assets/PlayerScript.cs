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

    public float moveSpeedX = 0.5f; // X Movement Speed of Player
    public float moveSpeedY = 1f; // Y Movement Speed of Player
    private Vector2 moveDirection; // Direction of Player

    // Input System 
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;


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
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();

        jump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get and store the SpriteRenderer component attached to this GameObject.
        defaultSprite = spriteRenderer.sprite;            // Store the initial sprite from the SpriteRenderer as the default sprite.
        gameObject.name = "stickManFighter";              // Rename the GameObject to "stickManFighter" for better identification in the hierarchy.
        animator = GetComponent<Animator>();              // Initializing the animator
        // playerRigidBody = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame - Process inputs here
    void Update()
    {
        //ProcessInputs();
        // Store the current velocity to avoid overwriting it
        Vector2 currentVelocity = playerRigidBody.velocity;

        currentVelocity.x = move.ReadValue<Vector2>().x * moveSpeedX;

        if (currentVelocity.x > 0 || currentVelocity.x < 0)
        {
         Debug.Log("Trying to Move");   
        }

        FlipSprite();

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump") /*jump.triggered*/ ) 
        {
            // First jump only allowed if on the stage, Allow a second jump while airborne (double jump)
            if (jumpCount == 0 || jumpCount == 1)
            {
                currentVelocity.y = jumpForce; // Apply upward velocity for first jump
                jumpCount++;
            
                animator.SetBool("isJumping", !isOnStage); // Lets the animator know that the player is now jumping
            }
        }
        // When jump key is released, set vert speed to 0 (Jump Cutting)
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))  /*jump.WasPressedThisFrame()*/ && currentVelocity.y > 0)
        {
            currentVelocity.y = currentVelocity.y * 0.20f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PerformAttack());
        }


        // Apply the velocity back to the Rigidbody2D
        playerRigidBody.velocity = currentVelocity;


        
    }

    // Fixed Update is called a set amount of times - Do physics here
    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(playerRigidBody.velocity.x));
        animator.SetFloat("yVelocity", playerRigidBody.velocity.y);

        // Checks to see if player is out of bounds and destroys player if true
        if (transform.position.x > outOfBoundsXRight || transform.position.x < outOfBoundsXLeft || transform.position.y < outOfBoundsY)
        {
            Debug.Log("You have been destroyed");
            Destroy(gameObject);
        }
    }

    void FlipSprite() 
    {
        if(isFacingRight && playerRigidBody.velocity.x < 0f || !isFacingRight && playerRigidBody.velocity.x > 0f) 
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    void ProcessInputs()
    {
        // getaxisraw 
        float moveX = Input.GetAxis("Horizontal") * moveSpeedX;
        //float moveY = 0f * moveSpeedY;

        // moveDirection = new Vector2 (moveX, playerRigidBody.velocity.y);
    }

    // void Move()
    // {
    //     playerRigidBody.velocity = new Vector2(moveDirection.x, moveDirection.y);
    // }

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

    void OnCollisionEnter2D(Collision2D collision)      // Checks if player is on the stage
    {
        if (collision.gameObject == stage)
        {
            isOnStage = true;
            jumpCount = 0;
        }
    }

    // void OnTriggerEnter2D(Collision2D collision)      // Checks if player is on the stage
    // {
    //     if (collision.gameObject == stage)
    //     {
    //         isOnStage = true;
    //         jumpCount = 0;
    //         animator.SetBool("isJumping", !isOnStage); // Lets the animator know that the player is now jumping

    //     }
    // }

    void OnCollisionExit2D(Collision2D collision)       // Sets on stage to false when player leaves the stage
    {
        if (collision.gameObject == stage)
        {
            isOnStage = false;
        }
    }
}
