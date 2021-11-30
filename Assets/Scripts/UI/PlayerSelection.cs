using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    SelectionHUD[] selectionHUDs;

    float waitTime = 3f;

	private void Awake()
	{
        selectionHUDs = GetComponentsInChildren<SelectionHUD>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckToStartGame();

        for (int i = 0; i < 6; i++)
		{
            if (Input.GetButtonDown("Attack" + i))
			{
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
            if (hud.joinedHud.activeSelf) joined++;
            if (!hud.player.ready) canContinue = false;
        }

        if (joined >= 2 && canContinue)
        {
            waitTime -= Time.deltaTime;
            // Display it on screen
        }
        else
        {
            waitTime = 3f;
        }

        if (waitTime <= 0f)
        {
            foreach (PlayerScript player in GameManager.current.players)
			{
                if (!player.ready)
                {
                    player.gameObject.SetActive(false);
                    player.playerHud.gameObject.SetActive(false);
                    player.intermissionHud.gameObject.SetActive(false);
                }
			}
            // Start the game
        }
    }
}
