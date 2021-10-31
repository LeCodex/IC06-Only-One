using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed;

    public Vector2 direction { private set; get; }
    public PlayerScript owner { private set; get; }
    public ProjectileWeapon weapon { private set; get; }

    protected Rigidbody2D rb;

	private void Awake()
	{
        rb = GetComponent<Rigidbody2D>();
	}

	public void Claim(PlayerScript player)
	{
        owner = player;
        weapon = (ProjectileWeapon)player.controller.weapon;
	}

	void FixedUpdate()
    {
        Tick();
    }

    public virtual void Tick() { }

    public void Throw(Vector2 dir)
    {
        direction = dir.normalized;
        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }
}
