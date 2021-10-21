using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateDead : ControllerStateBase
    {
        public override void EnterState(PlayerController context)
        {
            context.speed = 0;
            context.gameObject.layer = LayerMask.NameToLayer("Dead"); // Collide with walls, not players

            // Play death animation
        }

        public override void Update(PlayerController context)
        {
            
        }

        public override void ExitState(PlayerController context)
        {

        }
    }
}