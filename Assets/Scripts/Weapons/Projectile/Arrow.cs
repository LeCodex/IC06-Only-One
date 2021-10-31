using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
	public Collider2D solidCollider;

	void OnCollisionEnter2D(Collision2D collision)
	{
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);

		solidCollider.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		ProjectileWeapon wep = collision.GetComponent<ProjectileWeapon>();
		if (!wep) return;
		if (wep != weapon) return;

		weapon.ammo = Math.Min(weapon.maxAmmo, weapon.ammo + 1);

		Destroy(gameObject);
	}
}
