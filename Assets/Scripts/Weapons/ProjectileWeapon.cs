using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject ammo;
    
    Animator animator;
    Projectile projectile;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack(float charge)
    {
        projectile.Throw(transform.right);
        projectile.GetComponent<Transform>().parent = null;

        owner.controller.Unpause();
        projectile = null;
    }

    public override void Charge(float charge)
    {
        if (!projectile) projectile = Instantiate(ammo, attackPoint.position, transform.rotation, transform).GetComponent<Projectile>();

        owner.controller.Pause();
        Vector2 input = Input.GetAxisRaw("Horizontal" + owner.id) * Vector2.right + Input.GetAxisRaw("Vertical" + owner.id) * Vector2.up;
        if (input.sqrMagnitude > 0f) transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, input));
    }
}
