using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float attackRange = .5f;
    public LayerMask enemyLayers;
    public float chargeSlowdown;
    
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

	private void Update()
	{
        if (!owner) return;

        transform.localScale = new Vector3(owner.controller.lookingRight ? 1 : -1, 1, 1);
    }

	public override void Attack(float charge)
    {
        owner.controller.speed /= (1 - chargeSlowdown);
        
        animator.SetTrigger("Attack");

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D col in hit)
        {
            PlayerScript enemy = col.GetComponent<PlayerScript>();
            enemy.Damage(new DamageInfo(owner.id, enemy.id, attackDamage, "Melee"));
        }
    }

    public override void Charge(float charge)
    {
        owner.controller.speed *= (1 - chargeSlowdown);
    }
}
