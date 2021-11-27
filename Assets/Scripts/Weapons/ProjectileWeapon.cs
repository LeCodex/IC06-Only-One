using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
	public class ProjectileWeapon : Weapon
	{
		public Transform attackPoint;
		public GameObject ammunition;
		public int maxAmmo;
		public int ammo;
		public bool hideWhenOut;
		public AudioSource shootSound;

		Animator animator;
		Projectile projectile;
		GameObject aimingArrow;

		protected override void Start()
		{
			base.Start();
			ammo = maxAmmo;
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			if (!owner) return;

			transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
		}

		public override void Attack(float charge)
		{
			if (ammo == 0) return;
			UpdateAmmoCount(-1);

			projectile.transform.rotation = aimingArrow.transform.rotation;
			projectile.Throw(aimingArrow.transform.right);
			Destroy(aimingArrow);
			shootSound.Play();

			owner.controller.Unpause();
			projectile = null;
			aimingArrow = null;
		}

		public override void Charge(float charge)
		{
			if (ammo == 0) return;

			GetComponentInChildren<SpriteRenderer>().enabled = ammo > 1 || !hideWhenOut;

			if (!projectile) projectile = Instantiate(ammunition, attackPoint.position, Quaternion.identity).GetComponent<Projectile>();
			if (!aimingArrow) aimingArrow = Instantiate(GameManager.current.aimingArrow, owner.transform.position, transform.rotation);

			projectile.Claim(owner);
			owner.controller.Pause();
			Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
			if (input.sqrMagnitude > 0f) aimingArrow.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(owner.transform.right, input));
		}

		public override void SetOwner(PlayerScript player)
		{
			if (owner) owner.ammoDisplay.gameObject.SetActive(false);

			base.SetOwner(player);

			if (owner)
			{
				owner.ammoDisplay.gameObject.SetActive(true);
				UpdateAmmoCount(0);
			}
		}

		public void UpdateAmmoCount(int amount)
		{
			ammo = Math.Max(Math.Min(ammo + amount, maxAmmo), 0);
			if (owner) owner.ammoDisplay.value = (float)ammo / maxAmmo;

			GetComponentInChildren<SpriteRenderer>().enabled = ammo > 0 || !hideWhenOut;
		}

		protected override void OnEndRound()
		{
			UpdateAmmoCount(maxAmmo);
			base.OnEndRound();
		}
	}
}