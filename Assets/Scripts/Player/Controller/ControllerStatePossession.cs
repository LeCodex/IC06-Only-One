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
            context.ghostAnimator.Play("Possess");

            context.possessedHazard.OnPossess(context); // Play possession animation for the hazard

            context.rb.velocity = Vector2.zero;
        }

        public override void Update(PlayerController context)
        {
            // Move the ghost to make the camera work correctly
            context.transform.position = context.possessedHazard.transform.position;

            if (Input.GetButtonDown("Secondary" + context.player.id))
            {
                Unpossess(context);
            }
        }

        public override void FixedUpdate(PlayerController context)
        {
            float horizontal = Input.GetAxisRaw("Horizontal" + context.player.id);
            float vertical = Input.GetAxisRaw("Vertical" + context.player.id);

            if (horizontal > 0f) context.lookingRight = true; else if (horizontal < 0f) context.lookingRight = false;
            if (vertical > 0f) context.lookingUp = true; else context.lookingUp = false;

            context.possessedHazard.Tick();
        }

        public override void ExitState(PlayerController context)
        {
            context.possessedHazard.OnUnpossess(); // Play unpossession animation for the hazard
            context.possessedHazard = null;
        }

        public void Unpossess(PlayerController context)
        {
            context.transform.position = context.possessedHazard.transform.position;

            context.ChangeState(PlayerState.Ghost);
        }
    }
}