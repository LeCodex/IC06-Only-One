using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	[CreateAssetMenu(fileName = "Damage", menuName = "Perks/Damage", order = 1)]
	class DamagePerk : Perk
	{
		public float percent = .2f;

		public override void InitializeEvents()
		{
			GameEventSystem.current.onDamage += OnDamage;
		}

		void OnDamage(DamageInfo info)
		{
			if (info.attacker == player.id && info.cause != "Extra")
			{
				PlayerScript victim = GameManager.current.players[info.victim - 1];
				victim.ClearInvulnerability();
				victim.Damage(new DamageInfo(player.id, info.victim, (int)Math.Round(info.amount * percent), "Extra"));
			}
		}

		public override void RemoveEvents()
		{
			if (!GameEventSystem.current) return;
			GameEventSystem.current.onDamage -= OnDamage;
		}
	}
}