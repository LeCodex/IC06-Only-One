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
        public GameObject aimingArrow;
        public AudioSource firingSound;

        [SerializeField]
        float wait = 0f;
        Animator animator;
        Collider2D selfCollider;

        static string[] animationStates = { "Cannon_R", "Cannon_U", "Cannon_L", "Cannon_D" };

		private void Awake()
		{
            animator = GetComponentInChildren<Animator>();
            selfCollider = GetComponent<Collider2D>();
        }

		public override void OnPossess(PlayerController possessor)
        {
            base.OnPossess(possessor);
            wait = .5f; // Wait half a second before firing, mostly for animations
            aimingArrow.SetActive(true);
        }

        void Update()
        {
            wait = Math.Max(wait - Time.deltaTime, 0f);

            if (!ghost) return;

            if (Input.GetButton("Attack" + ghost.player.id) && wait == 0f)
            {
                Fire();
                wait = cooldown;
            }
        }

        public override void Tick()
        {
            aimingArrow.transform.Rotate(-Vector3.forward * Input.GetAxisRaw("Horizontal" + ghost.player.id) * turnSpeed * Time.fixedDeltaTime);
            int index = (int)Math.Floor(((aimingArrow.transform.rotation.eulerAngles.z + 45f) % 360f) / 90f);
            animator.Play(animationStates[index]);
        }

        void Fire()
        {
            firingSound.Play();

            Projectile projectile = Instantiate(ammo, transform.position + aimingArrow.transform.right, Quaternion.identity).GetComponent<Projectile>();
            projectile.Claim(ghost.player);
            
            foreach (Collider2D col in projectile.GetComponents<Collider2D>())
            {
                if (!col.isTrigger) Physics2D.IgnoreCollision(col, selfCollider);
            }

            projectile.Throw(aimingArrow.transform.right);
        }

		public override void OnUnpossess()
		{
			base.OnUnpossess();
            aimingArrow.SetActive(false);
        }
	}
}
