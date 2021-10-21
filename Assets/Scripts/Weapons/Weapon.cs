using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform attackPoint;

    protected PlayerScript owner;

    public abstract void Attack(float charge);
    public abstract void Charge(float charge);

    public virtual void SetOwner(PlayerScript player)
    {
        owner = player;
    }

    // Weapons should only collide with players, they don't move anyway outside of animations
	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (owner) return; // Don't steal weapons out of players' hands

        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
		if (!player) return;

        player.controller.TakeWeapon(this);
	}
}