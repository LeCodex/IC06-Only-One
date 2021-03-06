using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class SwordWeapon : MeleeWeapon
	{
		public float lungeSpeed;
		public float lungeDuration;
		public AudioSource dashSound;

		float lungeTime;

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();

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
		}

		public override void Attack(float charge)
		{
			if (recharge > 0f) return;

			if (charge > .2f) dashSound.Play();

			float newLungeTime = lungeDuration * charge;
			if (lungeTime == 0f) owner.controller.Knockback(newLungeTime, lungeSpeed * (float)Math.Pow(charge, 1.3f) * owner.controller.rb.velocity.normalized);
			lungeTime = newLungeTime;

			if (lungeTime == 0f) HitPeople();

			base.Attack(charge);
		}

		public override void Charge(float charge)
		{
			base.Charge(charge);
		}

		void HitPeople()
		{
			animator.SetTrigger("Attack");
			Vector3 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			if (input.magnitude == 0f) input = owner.controller.lookingRight ? Vector2.right : Vector2.left;
			TryAndHitPeople(input);
		}
	}
}