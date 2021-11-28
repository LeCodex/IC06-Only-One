using PerkSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
			GameManager.current.playHUD.SetActive(false);

			// Put controller in OOG state
			foreach(PlayerScript player in GameManager.current.players)
			{
				player.controller.ChangeState(PlayerState.OutOfGame);
				player.intermissionHud.Find("Ready Icon").gameObject.SetActive(false);

				// Regenerate the perks
				RectTransform perkList = (RectTransform)player.intermissionHud.Find("Panel").Find("Perk List");
				foreach (Transform child in perkList)
				{
					GameObject.Destroy(child.gameObject);
				}

				float i = 0;
				foreach (Perk perk in player.perks)
				{
					Vector3 pos = perkList.position + (i / (GameRules.current.PLAYER_MAX_PERKS - 1) - .5f) * perkList.rect.width * Vector3.right;
					GameObject o = GameObject.Instantiate(GameManager.current.perkHudObject, pos, Quaternion.identity, perkList);
					o.GetComponent<Image>().sprite = perk.sprite;
					i++;
				}

				player.intermissionHud.GetComponentInChildren<Slider>().value = (float)player.score / GameRules.current.GAME_MAX_SCORE;
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
			
			//...and for the next scene to be done loading
			if (GameManager.current.sceneLoading != null)
			{
				if (!GameManager.current.sceneLoading.isDone) ready = false;
			} 
			else
			{
				ready = false;
			}


			if (ready)
			{
				GameManager.current.ChangeState(RoundState.Play);
			}
		}

		public override void ExitState()
		{
			// Hide intermission screen
			GameManager.current.intermissionTransition.Play("Hide");
			GameManager.current.playHUD.SetActive(true);

			GameManager.current.sceneLoading = null;

			foreach (PlayerScript player in GameManager.current.players)
			{
				player.health = GameRules.current.PLAYER_MAX_HEALTH;
			}
		}
	}
}