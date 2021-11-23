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

		void Start()
		{
			animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			if (!owner) return;

			transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
		}

		public override void Attack(float charge)
		{
			owner.controller.speed /= 1 - chargeSlowdown;

			animator.SetTrigger("Attack");
		}

		public override void Charge(float charge)
		{
			owner.controller.speed *= 1 - chargeSlowdown;
		}

		void HitPeople()
		{
			Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Melee"));
			}
		}
	}
}