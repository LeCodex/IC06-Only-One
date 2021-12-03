using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	[CreateAssetMenu(fileName = "Defense", menuName = "Perks/Defense", order = 1)]
	class DefensePerk : Perk
	{
		public string title = "Defense";
		public float percent = .2f;

		public override void InitializeEvents()
		{
			player.resistance += percent;
		}

		public override void RemoveEvents()
		{
			player.resistance -= percent;
		}
	}
}