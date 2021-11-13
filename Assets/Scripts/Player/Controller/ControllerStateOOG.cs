using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateOOG : ControllerStateBase
    {
        public override void EnterState(PlayerController context)
        {
            // This state is only for when players are out of the game, i.e. in the intermission or during the player selection
            context.rb.velocity = Vector2.zero;

            // Safety
            context.player.ready = false;
        }

        public override void Update(PlayerController context)
        {
            if (Input.GetButtonDown("Attack" + context.player.id))
            {
                context.player.ready = !context.player.ready;
                context.player.intermissionHud.Find("Ready Icon").gameObject.SetActive(context.player.ready);
            }
        }

        public override void FixedUpdate(PlayerController context)
        {
            // Don't move
        }

        public override void ExitState(PlayerController context)
        {
            
        }
    }
}