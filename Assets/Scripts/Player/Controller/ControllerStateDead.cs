using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateDead : ControllerStateBase
    {
        float timeToGhostification = 1f;
        float timer = 0f;

        public override void EnterState(PlayerController context)
        {
            timer = timeToGhostification;

            context.speed = 0;
            context.gameObject.layer = LayerMask.NameToLayer("Dead"); // Collide with walls, not players

            // Play death animation
        }

        public override void Update(PlayerController context)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
			{
                context.player.ChangeState(PlayerState.Ghost);
			}
        }

        public override void ExitState(PlayerController context)
        {

        }
    }
}