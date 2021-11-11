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

	private void Start()
	{
        rb = GetComponent<Rigidbody2D>();

        GameEventSystem.current.onEndRound += OnEndRound;
	}

	public void Claim(PlayerScript player)
	{
        owner = player;
        weapon = (ProjectileWeapon)player.controller.weapon;

        foreach(Collider2D col in GetComponents<Collider2D>())
		{
            if (!col.isTrigger) Physics2D.IgnoreCollision(col, owner.GetComponent<BoxCollider2D>());
        }
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
