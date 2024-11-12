using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState 
{
    public string name;

    public string stateName;
    protected PlayerStateMachine playerStateMachine;
    // Start is called before the first frame update
    public PlayerBaseState(string name, PlayerStateMachine stateMachine)
    {
        this.name = name;
        this.playerStateMachine = stateMachine;
    }

    public virtual void Enter(string previousState) { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
