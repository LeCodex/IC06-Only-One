using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject ammunition;
    public int maxAmmo;
    public int ammo;
    public bool hideWhenOut;

    Animator animator;
    Projectile projectile;

    void Start()
    {
        ammo = maxAmmo;
        animator = GetComponent<Animator>();
    }

	private void Update()
	{
        if (!owner) return;

        if (owner.controller.rb.velocity.magnitude > 0f) transform.rotation = Quaternion.Euler(0f, 0f, owner.controller.lookingRight ? 0f : 180f);
    }

	public override void Attack(float charge)
    {
        if (charge == 0) return;

        projectile.Throw(transform.right);
        projectile.GetComponent<Transform>().SetParent(null);

        owner.controller.Unpause();
        projectile = null;
    }

    public override void Charge(float charge)
    {
        if (ammo == 0) return;
        UpdateAmmoCount(-1);

        if (!projectile) projectile = Instantiate(ammunition, attackPoint.position, transform.rotation, transform).GetComponent<Projectile>();

        projectile.Claim(owner);
        owner.controller.Pause();
        Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
        if (input.sqrMagnitude > 0f) transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(owner.transform.right, input));
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
        owner.ammoDisplay.value = (float)ammo / maxAmmo;

        GetComponentInChildren<SpriteRenderer>().enabled = ammo > 0 || !hideWhenOut;
    }

    protected override void OnEndRound()
    {
        UpdateAmmoCount(maxAmmo);
        base.OnEndRound();
    }
}
