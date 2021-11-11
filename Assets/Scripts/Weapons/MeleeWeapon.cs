using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float attackRange = .5f;
    public LayerMask enemyLayers;
    
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void Attack(float charge)
    {
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
        
    }
}
