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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
