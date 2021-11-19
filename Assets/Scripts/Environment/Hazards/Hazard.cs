using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaEnvironment
{
    public abstract class Hazard : MonoBehaviour
    {
        public Sprite hintSprite;

        public PlayerController ghost { private set; get; } = null;

        int playersInRange = 0;
        ButtonHint hint;

		private void Start()
		{
            hint = GetComponent<ButtonHint>();
		}

		public virtual void OnPossess(PlayerController possessor)
        {
            hint.HideHint();
            ghost = possessor;
        }

        public abstract void Tick(); // Fired every FixedUpdate of the player that possesses this Hazard

        public virtual void OnUnpossess()
        {
            hint.ShowHint("Possess", hintSprite);
            ghost = null;
        }


        // IMPORTANT: Hazard triggers should only detect players, even alive ones
        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            if (!controller) return;
            if (controller.player.playerState != PlayerState.Ghost) return;

            controller.FindHazard(this);
            if (!ghost) hint.ShowHint("Possess", hintSprite);
            playersInRange++;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            PlayerController controller = collision.GetComponent<PlayerController>();
            if (!controller) return;

            // Just to be safe, we always try and unfind hazards
            controller.UnfindHazard(this);
            playersInRange--;
            if (playersInRange == 0) hint.HideHint();
        }
    }
}