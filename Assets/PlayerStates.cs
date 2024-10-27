using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerStates
{
    public enum PlayerState
    {
        Idle,
        Grounded,
        Airborne,
        Attacking,
        TakingDamage,
        Dead
    }
    public enum PlayerJumpState
    {
        JumpHeld,
        JumpReleased
    }
}
