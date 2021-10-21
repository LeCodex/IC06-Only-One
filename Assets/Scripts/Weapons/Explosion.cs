using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public float radius;
	public int damage;
	public LayerMask enemyLayers;

	ParticleSystem system;

	private void Awake()
	{
		system = GetComponent<ParticleSystem>();
	}

	public void Explode(PlayerScript player)
	{
		if (system) system.Play();

		Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);
		foreach (Collider2D col in hit)
		{
			PlayerScript enemy = col.GetComponent<PlayerScript>();
			enemy.Damage(new DamageInfo(player.id, enemy.id, damage, "Explosion"));
		}
		
		Destroy(gameObject);
	}
}
