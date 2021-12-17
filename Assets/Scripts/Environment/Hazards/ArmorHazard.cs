using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

namespace ArenaEnvironment
{
	// An armor dead players can get control of
	public class ArmorHazard : Hazard
	{
		public float speed;

		SwordWeapon weapon;
		Rigidbody2D rb;
		Animator animator;

		private void Awake()
		{
			weapon = GetComponentInChildren<SwordWeapon>();
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponentInChildren<Animator>();
		}

		public override void OnPossess(PlayerController possessor)
		{
			base.OnPossess(possessor);
			rb.bodyType = RigidbodyType2D.Dynamic;
			weapon.SetOwner(possessor.player);
		}

		private void Update()
		{
			if (!ghost) return;

			if (Input.GetButtonDown("Attack" + ghost.player.id))
			{
				weapon.Attack(0);
			}
		}

		public override void Tick()
		{
			if (!ghost) return;

			rb.velocity = (Input.GetAxisRaw("Horizontal" + ghost.player.id) * Vector2.right + Input.GetAxisRaw("Vertical" + ghost.player.id) * Vector2.up) * speed * Time.fixedDeltaTime;

			if (rb.velocity.magnitude > 0f) animator.Play("Walk" + ghost.GetAnimationStateDirection()); else animator.Play("Idle" + ghost.GetAnimationFlipHorizontal());
			weapon.render.sortingOrder = ghost.lookingUp ? -1 : 0;
		}

		public override void OnUnpossess()
		{
			base.OnUnpossess();
			animator.Play("IdleR");
			weapon.SetOwner(null);
			weapon.GetComponent<CircleCollider2D>().enabled = false;
			rb.bodyType = RigidbodyType2D.Kinematic;
			rb.velocity = Vector2.zero;
		}
	}
}
