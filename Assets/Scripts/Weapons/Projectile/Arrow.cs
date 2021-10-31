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
		transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, collision.contacts[0].normal), Vector3.forward);

		solidCollider.enabled = false;
	}
}
