using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : Projectile
{
	Explosion explosion;

	private void Awake()
	{
		explosion = GetComponent<Explosion>();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		explosion.Explode(owner);
	}
}
