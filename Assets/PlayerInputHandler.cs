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
        public InputAction neutralLight; // neutral ground/air J/West button
        public InputAction forwardLight; // moving ground/air J/West button
        public InputAction downLight; // down ground/air J/West button
        public InputAction neutralUpHeavy; // neutral/up ground/air I/L/North/East button
        public InputAction forwardHeavy; // forward ground/air I/L/North/East button
        public InputAction downHeavy; // down ground/air I/L/North/East button
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
        playerControls.neutralLight = playerInput.actions["NeutralLight"];
        playerControls.forwardLight = playerInput.actions["ForwardLight"];
        playerControls.downLight = playerInput.actions["DownLight"];
        playerControls.neutralUpHeavy = playerInput.actions["NeutralUpHeavy"];
        playerControls.forwardHeavy = playerInput.actions["ForwardHeavy"];
        playerControls.downHeavy = playerInput.actions["DownHeavy"];

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
