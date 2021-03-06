using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameRoundState
{
	public class RoundStatePlay : RoundStateBase
	{
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
				player.health = GameRules.current.PLAYER_MAX_HEALTH;
				player.controller.Unpause();

				Transform target = spawns[Random.Range(0, spawns.Count)];
				spawns.Remove(target);

				player.transform.position = target.position;
			}

			GameManager.current.roundEnded = false;
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

			if (!GameManager.current.roundEnded)
			{
				// Detect pause
				if (!GameManager.current.pauseMenu.shown)
				{
					foreach (PlayerScript player in GameManager.current.players)
					{
						if (Input.GetButton("Menu"))
						{
							GameManager.current.pauseMenu.Show();
						}
					}
				}

				// Detect win
				if (alive <= 1)
				{
					GameManager.current.winner = lastOne;
					RoundState nextState = RoundState.Intermission;
					if (lastOne)
					{
						lastOne.score++;
						if (lastOne.score >= GameRules.current.GAME_MAX_SCORE) nextState = RoundState.Win;
					}

					GameManager.current.roundEnded = true;

					GameManager.current.ChangeState(nextState, 3f);
				}
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