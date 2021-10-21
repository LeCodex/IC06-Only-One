using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
	void OnCollisionEnter2D(Collision2D collision)
	{
		speed = 0;
	}
}
