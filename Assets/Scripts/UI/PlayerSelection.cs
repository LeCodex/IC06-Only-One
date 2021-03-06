using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public Text title;
    public Fade fadeToBlack;
    public SettingsMenu settings;

    SelectionHUD[] selectionHUDs;
    float waitTime = 3f;

	private void Awake()
	{
        selectionHUDs = GetComponentsInChildren<SelectionHUD>();
	}

	private void Start()
	{
        GameManager.current.LoadNextLevel();
    }

	// Update is called once per frame
	void Update()
    {
        CheckToStartGame();

        for (int i = 1; i <= selectionHUDs.Length; i++)
		{
            if (Input.GetButtonDown("Attack" + i))
			{
                bool alreadyExists = false;
                foreach (SelectionHUD hud in selectionHUDs)
				{
                    if (hud.player.id == i && hud.joinedHud.activeSelf)
					{
                        alreadyExists = true;
                        break;
					}
				}

                if (alreadyExists) continue;

                foreach (SelectionHUD hud in selectionHUDs)
                {
                    if (!hud.joinedHud.activeSelf)
                    {
                        hud.Join(i);
                        break;
                    }
                }
			}
		}

        if (Input.GetButtonDown("Menu"))
		{
            StartCoroutine(LoadMenuScene());
		}

        if (Input.GetButtonDown("Settings"))
		{
            gameObject.SetActive(false);
            settings.gameObject.SetActive(true);
		}
    }

    void CheckToStartGame()
	{
        int joined = 0;
        bool canContinue = true;

        foreach (SelectionHUD hud in selectionHUDs)
        {
            if (hud.joinedHud.activeSelf)
            {
                joined++;
                if (!hud.ready) canContinue = false;
            }
        }

        if (joined >= 2 && canContinue)
        {
            waitTime -= Time.deltaTime;

            // Display it on screen
            title.text = "DEBUT DANS " + (Math.Floor(waitTime * 10f) / 10f).ToString().PadRight(2, '0');
            // Debug.Log(waitTime);
        }
        else
        {
            waitTime = 3f;
            title.text = "REJOIGNEZ LA PARTIE";
        }

        if (waitTime <= 0f)
        {
            StartGame();
        }
    }

    void StartGame()
	{
        CinemachineTargetGroup targetGroup = FindObjectOfType<CinemachineTargetGroup>();
        foreach (SelectionHUD hud in selectionHUDs)
        {
            if (hud.ready)
            {
                hud.player.playerHud.gameObject.SetActive(true);
                hud.player.intermissionHud.gameObject.SetActive(true);
                targetGroup.AddMember(hud.player.transform, 1, 5);
            }
            else
            {
                hud.player.gameObject.SetActive(false);
                hud.player.playerHud.gameObject.SetActive(false);
                hud.player.intermissionHud.gameObject.SetActive(false);
                GameManager.current.players.Remove(hud.player);
            }
        }

        GameManager.current.ChangeState(RoundState.Play);
        GameRules.current.PLAYER_ALIVE_SPEED = (int)Math.Round(GameRules.current.PLAYER_ALIVE_SPEED * settings.speedFactor);
        GameRules.current.PLAYER_GHOST_SPEED = (int)Math.Round(GameRules.current.PLAYER_GHOST_SPEED * settings.speedFactor);
        transform.parent.gameObject.SetActive(false);
    }

    /*public void MoveHudsBack()
	{
        for (int i = 0; i < selectionHUDs.Length - 1; i++)
        {
            SelectionHUD hud = selectionHUDs[i];
            SelectionHUD nextHud = selectionHUDs[i + 1];
            if (!hud.joinedHud.activeSelf && nextHud.joinedHud.activeSelf)
			{
                hud.player.ready = nextHud.player.ready;
                hud.player.render.sprite = nextHud.player.render.sprite;
                hud.Join(nextHud.player.id);

                nextHud.Leave(true);
			}
        }
    }*/

    IEnumerator LoadMenuScene()
    {
        fadeToBlack.goalFade = 1f;

        yield return new WaitForSeconds(.6f);

        SceneManager.LoadSceneAsync(0);
    }
}
