using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public abstract class Projectile : MonoBehaviour
	{
		public float speed;
		public int attackDamage;
		public Rigidbody2D rb;
		public Collider2D solidCollider;
		public Collider2D wallCollider;

		public Vector2 direction { private set; get; }
		public PlayerScript owner { private set; get; }
		public ProjectileWeapon weapon { private set; get; }

		protected bool thrown = false;

		private void Awake()
		{
			GameEventSystem.current.onEndRound += OnEndRound;
			thrown = false;
		}

		public void Claim(PlayerScript player)
		{
			owner = player;
			if (player.controller.weapon is ProjectileWeapon) weapon = (ProjectileWeapon)player.controller.weapon;

			foreach (Collider2D col in GetComponents<Collider2D>())
			{
				if (!col.isTrigger) Physics2D.IgnoreCollision(col, owner.controller.projectileCollider);
			}
		}

		void FixedUpdate()
		{
			Tick();
		}

		public virtual void Tick() { }

		public void Throw(Vector2 dir)
		{
			thrown = true;

			if (solidCollider) solidCollider.enabled = true;
			if (wallCollider) wallCollider.enabled = true;

			direction = dir.normalized;
			rb.velocity = direction * speed;
		}

		void OnEndRound()
		{
			// Remove projectiles once round ends
			Destroy(gameObject);
		}

		void OnDestroy()
		{
			if (!GameEventSystem.current) return;

			GameEventSystem.current.onEndRound -= OnEndRound;
		}
	}
}