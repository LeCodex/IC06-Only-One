using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameRoundState
{
	public class RoundStatePlay : RoundStateBase
	{
		bool roundEnded = false;
		float waitForReset;

		public override void EnterState()
		{
			GameManager.current.intermissionTransition.Play("Hide");

			Transform spawnPoints = GameObject.FindWithTag("Spawnpoints").transform;
			List<Transform> spawns = new List<Transform>(spawnPoints.GetComponentsInChildren<Transform>().Where(x => x.gameObject.transform.parent != spawnPoints.parent).ToArray());

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
			waitForReset = 1f;

			// Call the event
			GameEventSystem.current.OnStartRound();
		}

		public override void Update()
		{
			if (waitForReset > 0f)
			{
				waitForReset -= Time.deltaTime;

				if (waitForReset <= 0f)
				{
					waitForReset = 0f;

					// Revert controllers to correct state
					foreach (PlayerScript player in GameManager.current.players)
					{
						player.ChangeState(PlayerState.Alive);
					}
				}
				else
				{
					return;
				}
			}

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

			if (alive <= 1 && !roundEnded)
			{
				GameManager.current.winner = lastOne;
				if (lastOne) lastOne.score++;

				roundEnded = true;
				
				GameManager.current.ChangeState(lastOne.score < GameRules.current.GAME_MAX_SCORE ? RoundState.Intermission : RoundState.Win, 3f);
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