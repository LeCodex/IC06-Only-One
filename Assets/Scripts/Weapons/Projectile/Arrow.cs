using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class Arrow : Projectile
	{
		public Collider2D pickupCollider;

		void OnCollisionEnter2D(Collision2D collision)
		{
			if (!thrown) return;

			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0;
			transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);

			solidCollider.enabled = false;
			wallCollider.enabled = false;
			pickupCollider.enabled = true;

			PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
			if (!player) return;

			player.controller.Knockback(.5f, (player.transform.position - transform.position).normalized * 50f);
			player.Damage(new DamageInfo(owner.id, player.id, attackDamage, "Arrow"));
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
			PlayerScript player = collision.GetComponentInParent<PlayerScript>();
			if (!player) return;
			Weapon wep = player.controller.weapon;
			if (!(wep is ProjectileWeapon)) return;

			((ProjectileWeapon)wep).UpdateAmmoCount(1);

			Destroy(gameObject);
		}
	}
}