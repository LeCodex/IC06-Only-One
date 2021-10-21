using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject ammo;
    public float throwSpeed;
    
    Animator animator;
    Projectile projectile;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack(float charge)
    {
        projectile.Throw(transform.right * throwSpeed);
    }

    public override void Charge(float charge)
    {
        projectile = Instantiate(ammo, transform.position, transform.rotation).GetComponent<Projectile>();
    }
}
