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

		MeleeWeapon weapon;
		Rigidbody2D rb;

		private void Awake()
		{
			weapon = GetComponentInChildren<MeleeWeapon>();
			rb = GetComponent<Rigidbody2D>();
		}

		public override void OnPossess(PlayerController possessor)
		{
			base.OnPossess(possessor);
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
			rb.velocity = (Input.GetAxisRaw("Horizontal" + ghost.player.id) * Vector2.right + Input.GetAxisRaw("Vertical" + ghost.player.id) * Vector2.up) * speed * Time.fixedDeltaTime;
		}

		public override void OnUnpossess()
		{
			base.OnUnpossess();
			rb.velocity = Vector2.zero;
		}
	}
}
