using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
	void OnCollisionEnter2D(Collision2D collision)
	{
		speed = 0;

		BoxCollider2D box = GetComponent<BoxCollider2D>();

		if (box)
		{
			box.enabled = false;
		}
	}
}
