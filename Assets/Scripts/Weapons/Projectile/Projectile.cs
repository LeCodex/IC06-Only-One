using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed;

    public Vector2 direction { private set; get; }
    public PlayerScript owner { private set; get; }

    public void Claim(PlayerScript player)
	{
        owner = player;
	}

	void FixedUpdate()
    {
        transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;

        Tick();
    }

    public virtual void Tick() { }

    public void Throw(Vector2 dir)
    {
        direction = dir.normalized;
    }
}
