using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    SelectionHUD[] selectionHUDs;

    float waitTime = 3f;
    int joined = 0;

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

        foreach (PlayerScript player in GameManager.current.players)
		{
            if (Input.GetButtonDown("Attack" + player.id))
			{
                selectionHUDs[joined].Join(player.id);
                joined++;
			}
		}
    }

    void CheckToStartGame()
	{
        bool canContinue = true;

        foreach (SelectionHUD hud in selectionHUDs)
        {
            if (!GameManager.current.players[hud.id - 1].ready) canContinue = false;
        }

        if (joined >= 2 && canContinue) waitTime -= Time.deltaTime; else waitTime = 3f;

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
