using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateOOG : ControllerStateBase
    {
        float timeToReset;

        public override void EnterState(PlayerController context)
        {
            // This state is only for when players are out of the game, i.e. in the intermission or during the player selection
            context.rb.velocity = Vector2.zero;

            // Safety
            context.player.ready = false;

            // Save players
            context.projectileCollider.enabled = false;
            context.solidCollider.enabled = false;

            // Reset visuals
            timeToReset = 1.5f;
        }

        public override void Update(PlayerController context)
        {
            if (Input.GetButtonDown("Attack" + context.player.id))
            {
                context.player.ready = !context.player.ready;
                context.player.intermissionHud.transform.Find("Ready Icon").gameObject.SetActive(context.player.ready);
            }
            
            if (timeToReset < 0f)
			{
                timeToReset = 0f;
                if (context.weapon) context.weapon.gameObject.SetActive(true);
                
                context.aliveAnimator.gameObject.SetActive(true);
                context.aliveAnimator.transform.rotation = Quaternion.identity;
                context.aliveAnimator.Play("IdleR");

                context.ghostAnimator.gameObject.SetActive(false);
            }
            else if (timeToReset > 0f)
			{
                timeToReset -= Time.deltaTime;
            }
        }

        public override void FixedUpdate(PlayerController context)
        {
            // Don't move
        }

        public override void ExitState(PlayerController context)
        {
            context.solidCollider.enabled = true;
        }
    }
}