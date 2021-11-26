using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class MeleeWeapon : Weapon
	{
		public float attackRange = .5f;
		public float attackSize = .5f;
		public LayerMask enemyLayers;
		public float chargeSlowdown;
		public float lungeSpeed;
		public float lungeDuration;

		Animator animator;
		float lungeTime;
		bool startCharge = true;

		protected override void Start()
		{
			base.Start();
			animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			if (!owner) return;

			if (lungeTime > 0f)
			{
				lungeTime -= Time.deltaTime;
			}

			if (lungeTime < 0f)
			{
				lungeTime = 0f;
				TryAndHitPeople();
			}

			transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
		}

		public override void Attack(float charge)
		{
			startCharge = true;
			owner.controller.speed += chargeSlowdown;

			float newLungeTime = lungeDuration * charge;
			if (lungeTime == 0f) owner.controller.Knockback(newLungeTime, lungeSpeed * (float)Math.Pow(charge, 1.3f) * owner.controller.rb.velocity.normalized);
			lungeTime = newLungeTime;
		}

		public override void Charge(float charge)
		{
			if (startCharge) owner.controller.speed -= chargeSlowdown;

			startCharge = false;
		}

		void TryAndHitPeople()
		{
			animator.SetTrigger("Attack");

			Vector3 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			if (input.magnitude == 0f) input = owner.controller.lookingRight ? Vector2.right : Vector2.left;
			Collider2D[] hit = Physics2D.OverlapCircleAll(owner.transform.position + input * attackRange, attackSize, enemyLayers);

			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				if (enemy == owner) continue;

				enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Melee"));
			}
		}
	}
}