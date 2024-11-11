using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    public struct PlayerActions
    { 
        public InputAction move; // joystick/WASD
        public InputAction jump; // Space/South Button
        public InputAction neutralGAttack; // neutral ground L/East
        public InputAction dashGAttack; // moving ground L/East
    }
    PlayerActions playerControls;

    private PlayerMovement playerMovement;
    private PlayerInput playerInput; 
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        // Finds all objects with playermovement component attached
        var playerMovements = FindObjectsOfType<PlayerMovement>();

        // retrieves player index 
        var index = playerInput.playerIndex;

        // Finds the PlayerMovement with the matching player index to associate it with this player
        playerMovement = playerMovements.FirstOrDefault(m => m.GetPlayerIndex() == index);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.currentVelocity = playerMovement.playerRigidBody.velocity;
        playerMovement.currentVelocity.x = playerControls.move.ReadValue<Vector2>().x * playerMovement.moveSpeed;
        playerMovement.playerRigidBody.velocity = playerMovement.currentVelocity;
    }

    private void OnEnable()
    {
        // Subscribe to input actions
        playerControls.move = playerInput.actions["Move"];
        playerControls.jump = playerInput.actions["Jump"];
        playerControls.neutralGAttack = playerInput.actions["NeutralGAttack"];
        playerControls.dashGAttack = playerInput.actions["DashGAttack"];
        
        playerControls.jump.started += playerMovement.Jump;  // Track the jump press
        playerControls.jump.canceled += playerMovement.Jump; // Track the jump release

        //playerControls.neutralGAttack.started += NeutralGAttack;
        //playerControls.dashGAttack.started += DashGAttack;

    }
    // Unsubscribe all methods to avoid memory leaks
    private void OnDisable()
    {
        playerControls.jump.started -= playerMovement.Jump;
        playerControls.jump.canceled -= playerMovement.Jump;

        //playerControls.neutralGAttack.started -= NeutralGAttack;
        //playerControls.dashGAttack.started -= DashGAttack;
    }
   
}
