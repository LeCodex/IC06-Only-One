using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vampirism", menuName = "Perks/Vampirism", order = 1)]
class VampirismPerk : Perk
{
    public string title = "Vampirism";
    public int healthGain = 20;

    public override void InitializeEvents()
    {
        GameEventSystem.current.onDamage += OnDamage;
    }

    void OnDamage(DamageInfo info)
    {
        if (info.attacker == player.id)
        {
            player.Heal(healthGain, "Vampirism");
        }
    }

    public override void RemoveEvents()
    {
        GameEventSystem.current.onDamage -= OnDamage;
    }
}

