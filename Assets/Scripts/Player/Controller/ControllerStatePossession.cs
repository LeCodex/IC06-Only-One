using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStatePossession : ControllerStateBase
    {
        public override void EnterState(PlayerController context)
        {
            // Don't need to change the layer, as you should only change from the Ghost state which doesn't collide with anything except borders

            // Play possession animation for player

            context.possessedHazard.OnPossess(context); // Play possession animation for the hazard

            context.rb.velocity = Vector2.zero;
        }

        public override void Update(PlayerController context)
        {
            if (Input.GetButtonDown("Secondary" + context.player.id))
            {
                context.Unpossess();
            }
        }

        public override void FixedUpdate(PlayerController context)
        {
            context.possessedHazard.Tick();
        }

        public override void ExitState(PlayerController context)
        {
            context.possessedHazard.OnUnpossess(); // Play unpossession animation for the hazard
        }
    }
}