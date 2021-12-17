using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class ShieldWeapon : MeleeWeapon
	{
		public float chargeDuration;
		public float chargeSpeed;
		public int chargeAmount = 3;
		public float maniability;
		public bool hitPeopleDuringCharge;

		float chargeRemaining;
		int amountRemaining;
		Vector2 chargeDirection;
		GameObject aimingArrow;
		Vector3 oldPosition;

		protected override void Start()
		{
			base.Start();
			amountRemaining = chargeAmount;
		}

		protected override void Update()
		{
			base.Update();
		}

		private void FixedUpdate()
		{
			if (!owner) return;

			if (chargeRemaining > 0f)
			{
				swingSound.mute = true;
				animator.Play("Charge");
				owner.controller.rb.velocity = chargeSpeed * chargeDirection * Time.fixedDeltaTime;

				if (hitPeopleDuringCharge) HitPeople();

				if ((oldPosition - owner.transform.position).magnitude <= 0.05f && chargeRemaining < chargeDuration) chargeRemaining = -1f;
				chargeRemaining -= Time.fixedDeltaTime;

				Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
				if (input.magnitude > 0f) chargeDirection = Vector2.Lerp(chargeDirection, input, Math.Max(0, Vector2.Dot(chargeDirection, input)) * maniability);

				//aimingArrow.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(owner.transform.right, chargeDirection));
				//aimingArrow.transform.position = owner.transform.position;
			} 
			else if (chargeRemaining < 0f)
			{
				//Destroy(aimingArrow);
				swingSound.mute = false;
				owner.ClearInvulnerability();
				owner.controller.Unstun();
				chargeRemaining = 0;
				animator.Play("Attack");
				HitPeople();
			}

			oldPosition = owner.transform.position;
		}

		public override void Attack(float charge)
		{
			if (recharge > 0f || chargeRemaining > 0f) return;

			if (amountRemaining > 0)
			{
				owner.controller.Knockback(chargeDuration, Vector2.zero);
				chargeRemaining = chargeDuration;
				chargeDirection = aimingArrow.transform.right;
				owner.MakeInvulnerable(chargeDuration);
				UpdateChargesRemaining(-1);
			} 
			else
			{
				animator.Play("Attack");
				HitPeople();
			}

			Destroy(aimingArrow);

			base.Attack(charge);
		}

		public override void Charge(float charge)
		{
			if (recharge > 0f || chargeRemaining > 0f || amountRemaining == 0) return;
			if (!aimingArrow) aimingArrow = Instantiate(GameManager.current.aimingArrow, owner.transform.position, transform.rotation);

			Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			if (input.sqrMagnitude > 0f) aimingArrow.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(owner.transform.right, input));
			aimingArrow.transform.position = owner.transform.position;

			base.Charge(charge);
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

		void HitPeople()
		{
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

			bool hit = TryAndHitPeople(input);
			if (hit && chargeRemaining > 0f) chargeRemaining = -1f;
		}

		protected override void OnEndRound()
		{
			chargeRemaining = 0f;
			UpdateChargesRemaining(chargeAmount);
			base.OnEndRound();
		}
	}
}