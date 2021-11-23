using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class ShieldWeapon : Weapon
	{
		public float attackRange = .5f;
		public LayerMask enemyLayers;
		public float chargeSlowdown;
		public float chargeDuration;
		public float chargeSpeed;
		public int chargeAmount = 3;
		public bool hitPeopleDuringCharge;

		Animator animator;
		float chargeRemaining;
		int amountRemaining;
		Vector2 chargeDirection;

		protected override void Start()
		{
			base.Start();
			animator = GetComponentInChildren<Animator>();
			amountRemaining = chargeAmount;
		}

		private void Update()
		{
			if (!owner) return;

			transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
		}

		private void FixedUpdate()
		{
			if (!owner) return;

			if (chargeRemaining > 0f)
			{
				chargeRemaining -= Time.fixedDeltaTime;
				owner.controller.rb.velocity = chargeSpeed * chargeDirection * Time.fixedDeltaTime;

				if (hitPeopleDuringCharge) TryAndHitPeople();
			}

			if (chargeRemaining <= 0f && owner.controller.paused)
			{
				chargeRemaining = 0;
				TryAndHitPeople();
			}
		}

		public override void Attack(float charge)
		{
			owner.controller.speed += chargeSlowdown;

			if (chargeAmount > 0)
			{
				owner.controller.Knockback(chargeDuration, Vector2.zero);
				chargeRemaining = chargeDuration;
				chargeDirection = Vector2.right * Input.GetAxisRaw("Horizontal" + owner.id) + Vector2.up * Input.GetAxis("Vertical" + owner.id);
				chargeAmount--;
			} 
			else
			{
				TryAndHitPeople();
			}
		}

		public override void Charge(float charge)
		{
			if (chargeRemaining > 0f) return;
			owner.controller.speed -= chargeSlowdown;

			Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			if (input.sqrMagnitude > 0f) transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(owner.transform.right, input));
		}

		public override void SetOwner(PlayerScript player)
		{
			if (owner) owner.ammoDisplay.gameObject.SetActive(false);

			base.SetOwner(player);

			if (owner)
			{
				owner.ammoDisplay.gameObject.SetActive(true);
				UpdateChargesRemaining(0);
			}
		}

		public void UpdateChargesRemaining(int amount)
		{
			amountRemaining = Math.Max(Math.Min(amountRemaining + amount, chargeAmount), 0);
			owner.ammoDisplay.value = (float)amountRemaining / chargeAmount;
		}

		void TryAndHitPeople()
		{
			animator.SetTrigger("Attack");
			Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				if (enemy == owner) continue;

				enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Melee"));
			}
		}

		protected override void OnEndRound()
		{
			UpdateChargesRemaining(chargeAmount);
			base.OnEndRound();
		}
	}
}