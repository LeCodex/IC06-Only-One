using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateGhost : ControllerStateBase
    {
        public override void EnterState(PlayerController context)
        {
            context.speed = GameRules.current.PLAYER_GHOST_SPEED;
            context.gameObject.layer = LayerMask.NameToLayer("Ghost"); // Only collide with arena borders

            // Play "ghosting" animation
        }

        public override void Update(PlayerController context)
        {
            if (Input.GetButtonDown("Secondary" + context.player.id))
            {
                context.PossessClosest();
            }
        }

        public override void ExitState(PlayerController context)
        {

        }
    }
}