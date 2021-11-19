﻿using ArenaEnvironment;
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
                PossessClosest(context);
            }
        }

        public override void ExitState(PlayerController context)
        {

        }

        public void PossessClosest(PlayerController context)
        {
            // No hazard available
            if (context.availableHazards.Count == 0) return;
            
            Hazard closestHazard = context.availableHazards[0];
            foreach (Hazard hazard in context.availableHazards)
            {
                if (!hazard)
                {
                    context.availableHazards.Remove(hazard);
                    break;
                }

                // Take the closest unpossessed hazard
                if (Vector2.Distance(context.transform.position, hazard.transform.position) < Vector2.Distance(context.transform.position, closestHazard.transform.position) && !hazard.ghost)
                {
                    closestHazard = hazard;
                }
            }

            // No unpossessed hazard available
            if (!closestHazard) return;

            context.possessedHazard = closestHazard;
            context.ChangeState(PlayerState.Possession);
        }
    }
}