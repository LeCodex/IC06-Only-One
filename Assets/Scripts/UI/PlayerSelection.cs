using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public Text title;

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
            title.text = "DEBUT DANS " + (Math.Floor(waitTime * 10f) / 10f).ToString().PadRight(2, '0');
            Debug.Log(waitTime);
            // Display it on screen
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
            if (!hud.ready)
            {
                hud.player.gameObject.SetActive(false);
                hud.player.playerHud.gameObject.SetActive(false);
                hud.player.intermissionHud.gameObject.SetActive(false);
                hud.player.ready = true;
                GameManager.current.players.Remove(hud.player);
            }
            else
			{
                hud.player.gameObject.SetActive(true);
                hud.player.playerHud.gameObject.SetActive(true);
                hud.player.intermissionHud.gameObject.SetActive(true);
                targetGroup.AddMember(hud.player.transform, 1, 5);
            }
        }

        GameManager.current.ChangeState(RoundState.Play);
        gameObject.SetActive(false);
    }

    public void MoveHudsBack()
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
    }
}
