using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoundState
{
	public class RoundStateIntermission : RoundStateBase
	{
		public override void EnterState()
		{
			// Unload previous map, load next map
			GameManager.current.LoadNextLevel();

			// Call the event
			GameEventSystem.current.OnEndRound();

			// Show intermission screen over gameplay screen
			GameManager.current.intermissionTransition.Play("Show");

			// Put controller in OOG state
			foreach(PlayerScript player in GameManager.current.players)
			{
				player.controller.ChangeState(PlayerState.OutOfGame);
			}
		}

		public override void Update()
		{
			// Wait for every player to be ready...
			bool ready = true;
			foreach (PlayerScript player in GameManager.current.players)
			{
				if (!player.ready)
				{
					ready = false;
					break;
				}
			}

			//...and for scenes to be done loading
			if (ready && GameManager.current.sceneLoading.isDone)
			{
				GameManager.current.ChangeState(RoundState.Play);
			}
		}

		public override void ExitState()
		{
			// Hide intermission screen
			GameManager.current.intermissionTransition.Play("Hide");

			// Revert controller to correct state
			foreach (PlayerScript player in GameManager.current.players)
			{
				player.controller.ChangeState(player.playerState);
			}
		}
	}
}