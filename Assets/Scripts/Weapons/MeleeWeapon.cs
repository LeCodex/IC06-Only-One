using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class MeleeWeapon : Weapon
	{
		public float attackRange = .5f;
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
				HitPeople();
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

		void HitPeople()
		{
			animator.SetTrigger("Attack");

			Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Melee"));
			}
		}
	}
}