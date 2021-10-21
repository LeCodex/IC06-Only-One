using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaEnvironment
{
    public abstract class Hazard : MonoBehaviour
    {
        public PlayerController ghost { private set; get; }

        int playersInRange = 0;

        public virtual void OnPossess(PlayerController possessor)
        {
            ghost = possessor;
        }

        public abstract void Tick(); // Fired every FixedUpdate of the player that possesses this Hazard

        public virtual void OnUnpossess()
        {
            ghost = null;
        }


        // IMPORTANT: Hazard triggers should only detect players, even alive ones
        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            if (controller.player.playerState != PlayerState.Ghost) return;

            controller.FindHazard(this);
            playersInRange++;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            // Just to be safe, we always try and unfind hazards

            controller.UnfindHazard(this);
            playersInRange--;
        }
    }
}