using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform attackPoint;
    public int attackDamage = 30;

    protected PlayerScript owner;

    public abstract void Attack(float charge);
    public abstract void Charge(float charge);

    public virtual void SetOwner(PlayerScript player)
    {
        owner = player;
        GetComponent<CircleCollider2D>().enabled = !owner;
    }

    // Weapons should only collide with players, they don't move anyway outside of animations
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (owner) return; // Don't steal weapons out of players' hands

        PlayerScript player = collision.GetComponent<PlayerScript>();
		if (!player) return;

        player.controller.FindWeapon(this);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (!player) return;

        player.controller.UnfindWeapon(this);
    }
}