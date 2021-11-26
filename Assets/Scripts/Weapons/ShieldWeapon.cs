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
		bool startCharge = true;
		GameObject aimingArrow;
		Vector3 oldPosition;

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

				if ((oldPosition - owner.transform.position).magnitude <= 0.01f) chargeRemaining = -1f;
			}

			if (chargeRemaining < 0f)
			{
				owner.controller.Unstun();
				chargeRemaining = 0;
				TryAndHitPeople();
			}

			oldPosition = owner.transform.position;
		}

		public override void Attack(float charge)
		{
			if (chargeRemaining > 0f) return;

			startCharge = true;

			if (amountRemaining > 0)
			{
				owner.controller.speed += chargeSlowdown;
				owner.controller.Knockback(chargeDuration, Vector2.zero);
				chargeRemaining = chargeDuration;
				chargeDirection = aimingArrow.transform.right;
				UpdateChargesRemaining(-1);
			} 
			else
			{
				TryAndHitPeople();
			}

			Destroy(aimingArrow);
		}

		public override void Charge(float charge)
		{
			if (chargeRemaining > 0f || amountRemaining == 0) return;
			if (startCharge) owner.controller.speed -= chargeSlowdown;

			if (!aimingArrow) aimingArrow = Instantiate(GameManager.current.aimingArrow, owner.transform.position, transform.rotation);

			Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			if (input.sqrMagnitude > 0f) aimingArrow.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(owner.transform.right, input));
			aimingArrow.transform.position = owner.transform.position;

			startCharge = false;
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