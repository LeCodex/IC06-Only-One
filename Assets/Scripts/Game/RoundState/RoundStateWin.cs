using PerkSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRoundState
{
	public class RoundStateWin : RoundStateBase
	{
		float timeToReload;

		public override void EnterState()
		{
			// Show intermission screen over gameplay screen
			GameManager.current.winningTransition.Play("Show");

			// Put controller in OOG state
			foreach(PlayerScript player in GameManager.current.players)
			{
				player.controller.ChangeState(PlayerState.OutOfGame);
			}

			timeToReload = 5f;
		}

		public override void Update()
		{
			timeToReload -= Time.deltaTime;

			if (timeToReload <= 0f)
			{
				SceneManager.LoadSceneAsync(1);
			}
		}

		public override void ExitState()
		{
			// Never run
		}
	}
}