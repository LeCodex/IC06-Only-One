using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoundState
{
	public class RoundStatePlay : RoundStateBase
	{
		public override void EnterState()
		{
			// Unready players for next intermission and reactivate inputs;
			foreach (PlayerScript player in GameManager.current.players)
			{
				player.ready = false;
				player.controller.Unpause();
			}
		}

		public override void Update()
		{
			int alive = 0;

			// Check if players are all eliminated save for one
			foreach (PlayerScript player in GameManager.current.players)
			{
				if (player.playerState == PlayerState.Alive) alive += 1;
			}

			if (alive <= 1)
			{
				GameManager.current.ChangeState(RoundState.Intermission);
			}
		}

		public override void ExitState()
		{
			// Deactivate inputs from players
			foreach (PlayerScript player in GameManager.current.players)
			{
				player.controller.Pause();
			}
		}
	}
}