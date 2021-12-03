using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	[CreateAssetMenu(fileName = "Régénération", menuName = "Perks/Regen", order = 1)]
	class RegenPerk : Perk
	{
		public string title = "Régénération";
		public int amount = 10;
		public float delay = 2f;

		float time = 0f;

		public override void InitializeEvents()
		{
			time = delay;
		}

		public override void Tick()
		{
			time -= Time.deltaTime;

			if (time <= 0f)
			{
				player.Heal(amount, "Régénération");
				time = delay;
			}
		}

		public override void RemoveEvents()
		{
			
		}
	}
}