using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerState
{
    public abstract class ControllerStateBase
    {
        public abstract void EnterState(PlayerController context);
        public abstract void Update(PlayerController context);
        public virtual void FixedUpdate(PlayerController context)
        {
            float horizontal = Input.GetAxisRaw("Horizontal" + context.player.id);
            float vertical = Input.GetAxisRaw("Vertical" + context.player.id);

            if (horizontal > 0f) context.lookingRight = true; else if (horizontal < 0f) context.lookingRight = false;
            if (vertical > 0f) context.lookingUp = true; else context.lookingUp = false;

            if (context.stun > 0f)
			{
                return;
			}

            if (context.paused)
            {
                context.rb.velocity = Vector2.zero;
                return;
            }

            context.rb.velocity = (Vector2.right * horizontal + Vector2.up * vertical).normalized * context.speed * Time.fixedDeltaTime;

            // Debug.Log(Mathf.Sign(horizontal));
            // if (horizontal != 0f) context.transform.localScale.Set(Mathf.Sign(horizontal), 1f, 1f);
        }
        public abstract void ExitState(PlayerController context);
    }
}