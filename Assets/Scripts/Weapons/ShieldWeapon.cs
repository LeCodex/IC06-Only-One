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
		public float chargeDuration;
		public float chargeSpeed;
		public int chargeAmount = 3;

		Animator animator;
		float chargeRemaining;
		int amountRemaining;
		Vector2 chargeDirection;

		void Start()
		{
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
			if (chargeRemaining > 0f)
			{
				chargeRemaining -= Time.fixedDeltaTime;
				owner.controller.rb.velocity = chargeSpeed * chargeDirection;

				TryAndHitPeople();
			}

			if (chargeRemaining <= 0f && owner.controller.paused)
			{
				chargeRemaining = 0;
				owner.controller.Unpause();
			}
		}

		public override void Attack(float charge)
		{
			animator.SetTrigger("Attack");

			if (chargeAmount > 0)
			{
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

			owner.controller.Pause();

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
				UpdateAmmoCount(0);
			}
		}

		public void UpdateAmmoCount(int amount)
		{
			amountRemaining = Math.Max(Math.Min(amountRemaining + amount, chargeAmount), 0);
			owner.ammoDisplay.value = (float)amountRemaining / chargeAmount;

			GetComponentInChildren<SpriteRenderer>().enabled = amountRemaining > 0;
		}

		void TryAndHitPeople()
		{
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
			UpdateAmmoCount(chargeAmount);
			base.OnEndRound();
		}
	}
}