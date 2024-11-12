using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    private PlayerStateMachine _sm;

    public PlayerMovingState(PlayerStateMachine stateMachine) : base("Moving", stateMachine)
    { 
        this.stateName = "Moving";
        _sm = stateMachine;
    }

    public override void Enter(string previousState)
    {
        base.Enter(previousState);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
    }

    public override void UpdatePhysics()
    {
        /*var playerMain = _sm.playerMain;
        var input = playerMain.moveInput;

        var currentVelocity = playerMain.playerRigidBody.velocity;
        currentVelocity.x = input.x * playerMain.moveSpeed;
        playerMain.playerRigidBody.velocity = currentVelocity;
        */
        base.UpdatePhysics();
    }
}
