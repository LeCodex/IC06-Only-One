using System;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

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
            context.aliveAnimator.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            context.aliveAnimator.Play("IdleR");

            context.Unpause();
            if (context.weapon is ProjectileWeapon) ((ProjectileWeapon)context.weapon).KillAimingArrow();
        }

        public override void Update(PlayerController context)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f && !GameManager.current.roundEnded)
			{
                context.player.ChangeState(PlayerState.Ghost);
                context.player.resurrectSound.Play();
			}
        }

        public override void ExitState(PlayerController context)
        {
            
        }
    }
}