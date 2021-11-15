using System.Collections.Generic;
using UnityEngine;

namespace GameRoundState
{
	public class RoundStatePlay : RoundStateBase
	{
		bool roundEnded = false;

		public override void EnterState()
		{
			List<Transform> spawns = new List<Transform>(GameObject.FindWithTag("Spawnpoints").GetComponentsInChildren<Transform>());

			// Unready players for next intermission, reactivate inputs, and move them to spawnpoints
			foreach (PlayerScript player in GameManager.current.players)
			{
				player.ready = false;
				player.controller.Unpause();

				Transform target = spawns[Random.Range(0, spawns.Count)];
				spawns.Remove(target);

				player.transform.position = target.position;
			}

			roundEnded = false;

			// Call the event
			GameEventSystem.current.OnStartRound();
		}

		public override void Update()
		{
			int alive = 0;
			PlayerScript lastOne = null;
			
			// Check if players are all eliminated save for one
			foreach (PlayerScript player in GameManager.current.players)
			{
				if (player.playerState == PlayerState.Alive)
				{
					alive += 1;
					lastOne = player;
				}
			}

			alive = 2;

			if (alive <= 1 && !roundEnded)
			{
				if (lastOne) lastOne.score++;

				roundEnded = true;
				GameManager.current.ChangeState(RoundState.Intermission, 3f);
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