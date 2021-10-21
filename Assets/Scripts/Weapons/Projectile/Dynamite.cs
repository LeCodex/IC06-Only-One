using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : Projectile
{
	public float drag = .99f;
	public float fuse = 3f;

	Explosion explosion;

	private void Awake()
	{
		explosion = GetComponent<Explosion>();
	}

	private void Update()
	{
		fuse -= Time.deltaTime;

		if (fuse <= 0f) Explode();
	}

	public override void Tick()
	{
		speed *= drag;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		speed /= 2f;
		Throw(Vector2.Reflect(direction, collision.GetContact(0).normal));
	}

	void Explode()
	{
		explosion.Explode(owner);
		Destroy(gameObject);
	}
}
