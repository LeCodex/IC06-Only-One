﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateAlive : ControllerStateBase
    {

        public override void EnterState(PlayerController context)
        {
            context.speed = GameRules.current.PLAYER_ALIVE_SPEED;
            context.gameObject.layer = LayerMask.NameToLayer("Player"); // Collide with players and walls
        }

        public override void Update(PlayerController context)
        {
            if (context.weapon)
			{
                if (Input.GetButton("Attack" + context.player.id))
                {
                    context.charge = Math.Min(context.charge + Time.deltaTime, 1f);
                    context.weapon.Charge(context.charge);
                }
                else if (Input.GetButtonUp("Attack" + context.player.id))
                {
                    context.weapon.Attack(context.charge);
                    context.charge = 0f;
                } 
                else if (Input.GetAxisRaw("Horizontal" + context.player.id) != 0f)
			    {
                    context.weapon.transform.rotation = Input.GetAxisRaw("Horizontal" + context.player.id) > 0 ? Quaternion.AngleAxis(0f, Vector3.forward) : Quaternion.AngleAxis(180f, Vector3.forward);
			    }
			}
        }

		public override void FixedUpdate(PlayerController context)
		{
			base.FixedUpdate(context);

            if (context.rb.velocity.magnitude > 0f) context.PlayAnimation("Walk" + context.GetAnimationStateDirection()); else context.PlayAnimation("Idle" + context.GetAnimationFlipHorizontal());
		}

		public override void ExitState(PlayerController context)
        {
            
        }
    }
}