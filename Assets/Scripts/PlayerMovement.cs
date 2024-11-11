using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerStates;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 10f;
    public Vector2 currentVelocity = Vector2.zero;

    [SerializeField]
    private int playerIndex = 0; // index to differentiate the 2 players

    public Rigidbody2D playerRigidBody;
    public GameObject stage;
    public PlayerState playerState;

    //private Vector2 inputVector = Vector2.zero;

    // Jump Logic
    public PlayerJumpState playerJumpState;
    public int jumpCount = 0;
    public int jumpFrameCounter = 0;
    public bool didJump = false;
    //public bool jumpStarted = false;
    public bool shortHop = false;
    
    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 1.0f / 60.0f;  // Set FixedUpdate to run at 60 FPS
        playerJumpState = PlayerJumpState.JumpReleased;
        playerState = PlayerState.Idle;
        playerRigidBody.velocity = Vector2.zero;
    }
    public int GetPlayerIndex()
    {
        return playerIndex;
    }
    // Update is called once per frame
    void Update() // make this a virtual void
    {

    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // When jump button is released
            if (playerJumpState != PlayerJumpState.JumpReleased) playerJumpState = PlayerJumpState.JumpReleased;
        }
        if (jumpCount <= 1)
        {
            if (context.started) // jump pressed 
            {
                didJump = false;
                // When jump button is pressed
                playerJumpState = PlayerJumpState.JumpHeld; // TODO - CHANGE BACK ON COLL ENTER ON STAGE
                jumpFrameCounter = 0; // Reset frame counter
            }
            else if (context.canceled && !didJump) // jump released and havent jumped yet
            {
                shortHop = jumpFrameCounter < 5;
                // Determine if it's a short hop or a regular hop based on frame count
                PerformJump(shortHop);
            }
        }
    }
    public void PerformJump(bool isShortHop)
    {
        jumpCount++;
        didJump = true;
        if (isShortHop)
        {
            // Perform a short hop
            SetJumpVelocity(8f); // Lower jump force for short hop
        }
        else
        {
            // Perform a long hop
            SetJumpVelocity(12f); // Higher jump force for regular hop
        }
    }
    // FixedUpdate is called on a fixed time interval for physics updates
    public void SetJumpVelocity(float jumpForce)
    {
        currentVelocity.y = jumpForce; // Apply the upward force
        playerRigidBody.velocity = currentVelocity;
    }

    private void FixedUpdate() // make this a virtual void 
    {
        if (playerJumpState == PlayerJumpState.JumpHeld) jumpFrameCounter++; // track frames that jump button is held for
        if (jumpFrameCounter == 5 && playerState == PlayerState.Grounded) // bro took too long, long hop it is 
        {
            shortHop = false;
            PerformJump(shortHop);
        }
        if (jumpCount == 1 && !didJump && jumpFrameCounter == 2)
        {
            PerformJump(false); // if already in air then do a long hop
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TopStage"))
        {
            playerState = PlayerState.Grounded;
            jumpFrameCounter = 0; // Reset frame counter
            jumpCount = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TopStage"))
        {
            playerState = PlayerState.Airborne;
        }
    }


}
