using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public abstract class Weapon : MonoBehaviour
	{
		public Transform attackPoint;
		public int attackDamage = 30;
		public Sprite hintSprite;

		protected PlayerScript owner;

		protected virtual void Start()
		{
			GameEventSystem.current.onEndRound += _OnEndRound;
		}

		public abstract void Attack(float charge);
		public abstract void Charge(float charge);

		public virtual void SetOwner(PlayerScript player)
		{
			owner = player;
			owner.GetComponent<ButtonHint>().ShowHint("Attack", hintSprite);
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

		void _OnEndRound()
		{
			OnEndRound();
		}

		protected virtual void OnEndRound()
		{
			Debug.Log(owner);
			if (!owner) Destroy(gameObject);
		}

		void OnDestroy()
		{
			if (!GameEventSystem.current) return;

			GameEventSystem.current.onEndRound -= _OnEndRound;
		}
	}
}