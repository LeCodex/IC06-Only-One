using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	abstract public class MeleeWeapon : Weapon
	{
		public float attackRange = .5f;
		public float attackSize = .5f;
		public float attackCooldown = .5f;
		public float attackKnockback = 50f;
		public LayerMask enemyLayers;
		public float chargeSlowdown;
		public AudioSource swingSound;

		protected Animator animator;
		protected bool startCharge = true;
		protected float recharge = 0f;

		protected override void Start()
		{
			base.Start();
			animator = GetComponentInChildren<Animator>();
		}

		protected virtual void Update()
		{
			recharge = Math.Max(0f, recharge - Time.deltaTime);

			if (!owner) return;

			transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
		}

		public override void Attack(float charge)
		{
			if (recharge > 0f) return;

			startCharge = true;
			owner.controller.speed += chargeSlowdown;
			recharge = attackCooldown;
		}

		public override void Charge(float charge)
		{
			if (recharge > 0f) return;

			if (startCharge) owner.controller.speed -= chargeSlowdown;
			startCharge = false;
		}

		protected void TryAndHitPeople(Vector3 input)
		{
			swingSound.Play();
			animator.SetTrigger("Attack");
			
			Collider2D[] hit = Physics2D.OverlapCircleAll(owner.transform.position + input * attackRange, attackSize, enemyLayers);
			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				if (enemy == owner) continue;

				enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Melee"));
				enemy.controller.Knockback(1f, (enemy.transform.position - transform.position).normalized * attackKnockback);
			}
		}
	}
}