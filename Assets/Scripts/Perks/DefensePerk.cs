using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	[CreateAssetMenu(fileName = "Défense", menuName = "Perks/Defense", order = 1)]
	class DefensePerk : Perk
	{
		public string title = "Défense";
		public float percent = .2f;

		public override void InitializeEvents()
		{
			GameEventSystem.current.onDamage += OnDamage;
		}

		void OnDamage(DamageInfo info)
		{
			if (info.victim == player.id)
			{
				player.Heal((int)Math.Round(info.amount * percent), "Défense");
			}
		}

		public override void RemoveEvents()
		{
			if (!GameEventSystem.current) return;
			GameEventSystem.current.onDamage -= OnDamage;
		}
	}
}