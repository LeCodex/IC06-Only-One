using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

namespace ArenaEnvironment
{
	// Although called a Cannon, this could be reused for other hazards like flamethrowers (anything that just throws projectiles when you press attack)
	public class CannonHazard : Hazard
    {
        public GameObject ammo;
        public float cooldown;
        public float turnSpeed;

        [SerializeField]
        [ReadOnlySerialize]
        float wait = 0f;

        public override void OnPossess(PlayerController possessor)
        {
            base.OnPossess(possessor);
            wait = .5f; // Wait half a second before firing, mostly for animations
        }

        void Update()
        {
            wait = Math.Max(wait - Time.deltaTime, 0f);

            if (!ghost) return;

            if (Input.GetButtonDown("Attack" + ghost.player.id) && wait == 0f)
            {
                Fire();
                wait = cooldown;
            }
        }

        public override void Tick()
        {
            transform.Rotate(-Vector3.forward * Input.GetAxisRaw("Horizontal" + ghost.player.id) * turnSpeed * Time.fixedDeltaTime);
        }

        void Fire()
        {
            Projectile projectile = Instantiate(ammo, transform.position + transform.up * 2f, Quaternion.identity).GetComponent<Projectile>();
            projectile.Claim(ghost.player);

            Collider2D selfCollider = GetComponent<Collider2D>();
            foreach (Collider2D col in projectile.GetComponents<Collider2D>())
            {
                if (!col.isTrigger) Physics2D.IgnoreCollision(col, selfCollider);
            }

            projectile.Throw(transform.up);
        }
    }
}
