using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public class ControllerStateAlive : ControllerStateBase
    {
        bool presingDownAttack = false;

        public override void EnterState(PlayerController context)
        {
            context.speed = GameRules.current.PLAYER_ALIVE_SPEED;
            context.gameObject.layer = LayerMask.NameToLayer("Player"); // Collide with players and walls
            context.projectileCollider.enabled = true;

            if (context.weapon) context.weapon.gameObject.SetActive(true);

            context.aliveAnimator.gameObject.SetActive(true);
            context.ghostAnimator.gameObject.SetActive(false);
            context.player.ammoDisplay.transform.parent.gameObject.SetActive(true);

            presingDownAttack = false;
        }

        public override void Update(PlayerController context)
        {
            if (context.weapon)
			{
                if (Input.GetButton("Attack" + context.player.id))
                {
                    context.charge = Math.Min(context.charge + Time.deltaTime, 1f);
                    context.weapon.Charge(context.charge);
                    presingDownAttack = true;
                }
                else if (!Input.GetButtonUp("Attack" + context.player.id) && presingDownAttack)
                {
                    context.weapon.Attack(context.charge);
                    context.charge = 0f;
                    presingDownAttack = false;
			    }
			}

            if (Input.GetButtonDown("Secondary" + context.player.id))
            {
                context.TakeClosestWeapon();
            }

            context.weapon.render.sortingOrder = context.lookingUp ? -1 : 0;
        }

		public override void FixedUpdate(PlayerController context)
		{
			base.FixedUpdate(context);

            if (context.rb.velocity.magnitude > 0f) 
                context.PlayAnimation(context.aliveAnimator, "Walk" + context.GetAnimationStateDirection()); 
            else 
                context.PlayAnimation(context.aliveAnimator, "Idle" + context.GetAnimationFlipHorizontal());
		}

		public override void ExitState(PlayerController context)
        {
            context.projectileCollider.enabled = false;

            if (context.weapon) context.weapon.gameObject.SetActive(false);
            context.player.ammoDisplay.transform.parent.gameObject.SetActive(false);
        }
    }
}