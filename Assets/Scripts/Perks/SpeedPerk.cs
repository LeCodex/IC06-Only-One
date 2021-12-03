using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerkSystem
{
	[CreateAssetMenu(fileName = "Speed", menuName = "Perks/Speed", order = 1)]
	class SpeedPerk : Perk
	{
		public float speedGain = 0.2f;

		public override void InitializeEvents()
		{
			GameEventSystem.current.onChangeState += OnChangeState;
			OnChangeState(player.id, player.playerState);
		}

		void OnChangeState(int id, PlayerState state)
		{
			if (id == player.id && state == PlayerState.Alive) player.controller.speed += GameRules.current.PLAYER_ALIVE_SPEED * speedGain;
		}

		public override void RemoveEvents()
		{
			Debug.Log("Removed speed perk");
			if (player.playerState == PlayerState.Alive) player.controller.speed -= GameRules.current.PLAYER_ALIVE_SPEED * speedGain;

			if (!GameEventSystem.current) return;
			GameEventSystem.current.onChangeState -= OnChangeState;
		}
	}
}