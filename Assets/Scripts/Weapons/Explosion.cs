using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class Explosion : MonoBehaviour
	{
		public float radius;
		public int damage;
		public LayerMask enemyLayers;
		public GameObject system;

		public void Explode(PlayerScript player)
		{
			if (system) Instantiate(system, transform.position, Quaternion.identity);

			Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);
			foreach (Collider2D col in hit)
			{
				PlayerScript enemy = col.GetComponent<PlayerScript>();
				enemy.Damage(new DamageInfo(player.id, enemy.id, damage, "Explosion"));

				Vector2 knockbackDirection = enemy.transform.position - transform.position;
				enemy.controller.Knockback(.5f, knockbackDirection * radius / knockbackDirection.magnitude * 30f);
			}

			Destroy(gameObject);
		}
	}
}