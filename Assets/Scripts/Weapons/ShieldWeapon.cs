using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class ShieldWeapon : Weapon
	{
		public float attackRange = .5f;
		public float attackSize = .5f;
		public float attackCooldown = .5f;
		public float attackKnockback = 50f;
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
		float recharge;

		protected override void Start()
		{
			base.Start();
			animator = GetComponentInChildren<Animator>();
			amountRemaining = chargeAmount;
		}

		private void Update()
		{
			recharge = Math.Max(0f, recharge - Time.deltaTime);

			if (!owner) return;

			transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
		}

		private void FixedUpdate()
		{
			if (!owner) return;

			if (chargeRemaining > 0f)
			{
				owner.controller.rb.velocity = chargeSpeed * chargeDirection * Time.fixedDeltaTime;

				if (hitPeopleDuringCharge) TryAndHitPeople();

				if ((oldPosition - owner.transform.position).magnitude <= 0.01f && chargeRemaining < chargeDuration) chargeRemaining = -1f;
				chargeRemaining -= Time.fixedDeltaTime;
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
			if (recharge > 0f || chargeRemaining > 0f) return;

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

			recharge = attackCooldown;

			Destroy(aimingArrow);
		}

		public override void Charge(float charge)
		{
			if (recharge > 0f || chargeRemaining > 0f || amountRemaining == 0) return;
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
			if (owner) owner.ammoDisplay.value = (float)amountRemaining / chargeAmount;
		}

		void TryAndHitPeople()
		{
			animator.SetTrigger("Attack");

			Vector3 input;
			if (chargeRemaining > 0f)
			{
				input = chargeDirection;
			}
			else
			{
				input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			}
			if (input.magnitude == 0f) input = owner.controller.lookingRight ? Vector2.right : Vector2.left;

			Collider2D[] hit = Physics2D.OverlapCircleAll(owner.transform.position + input * attackRange, attackSize, enemyLayers);
			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				if (enemy == owner) continue;

				enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Shield Bash"));
				enemy.controller.Knockback(1f, (enemy.transform.position - transform.position).normalized * attackKnockback);
			}
		}

		protected override void OnEndRound()
		{
			UpdateChargesRemaining(chargeAmount);
			base.OnEndRound();
		}
	}
}