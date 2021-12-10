using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class Dynamite : Projectile
	{
		public float fuse = 3f;

		Explosion explosion;
		bool firstLine = true;

		private void Awake()
		{
			explosion = GetComponent<Explosion>();
		}

		private void Update()
		{
			rb.angularVelocity = rb.velocity.magnitude * 100f;

			fuse -= Time.deltaTime;

			if (fuse <= 0f) Explode();
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			speed = rb.velocity.magnitude / 2f;
			Throw(Vector2.Reflect(direction, collision.GetContact(0).normal));

			if (!firstLine) return;
			firstLine = false;

			PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
			if (player) Explode();
		}

		void Explode()
		{
			explosion.Explode(owner);
			weapon.UpdateAmmoCount(1);
			Destroy(gameObject);
		}
	}
}