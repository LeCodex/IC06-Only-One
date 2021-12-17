using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MenuManager
{
    public PlayerSelection selection;

    public float speedFactor { private set; get; } = 1f;

    protected override void ChooseCurrentOption()
    {
        // Debug.Log("Chose " + currentOption);
        switch(currentOption)
		{
            case 5:
                gameObject.SetActive(false);
                selection.gameObject.SetActive(true);

                UpdateOptionColor(new Color(255, 255, 255, 255));
                currentOption = 0;
                UpdateOptionColor(new Color(0, 255, 0, 255));
                break;
		}
    }

	protected override void LeftRightCurrentOption(int axis)
	{
		switch(currentOption)
		{
            case 0:
                GameRules.current.GAME_MAX_SCORE = Math.Min(Math.Max(3, GameRules.current.GAME_MAX_SCORE + axis), 10);
                UpdateCurrentOptionValue(GameRules.current.GAME_MAX_SCORE);
                break;

            case 1:
                GameRules.current.PLAYER_MAX_HEALTH = Math.Min(Math.Max(10, GameRules.current.PLAYER_MAX_HEALTH + 10 * axis), 200);
                UpdateCurrentOptionValue(GameRules.current.PLAYER_MAX_HEALTH);
                break;

            case 2:
                speedFactor = Math.Min(Math.Max(.5f, speedFactor + .1f * axis), 3f);
                UpdateCurrentOptionValue(speedFactor, "x");
                break;

            case 3:
                GameRules.current.GHOST_KILL_REGEN_FLAT = Math.Min(Math.Max(10, GameRules.current.GHOST_KILL_REGEN_FLAT + 10 * axis), 200);
                UpdateCurrentOptionValue(GameRules.current.GHOST_KILL_REGEN_FLAT);
                break;

            case 4:
                GameRules.current.PLAYER_MAX_PERKS = Math.Min(Math.Max(1, GameRules.current.PLAYER_MAX_PERKS + 1 * axis), 10);
                UpdateCurrentOptionValue(GameRules.current.PLAYER_MAX_PERKS);
                break;
        }
	}

    void UpdateCurrentOptionValue(float value, string supplement = "")
	{
        Text t = options.GetChild(currentOption).GetComponent<Text>();
        string[] splitted = t.text.Split(':');
        t.text = splitted[0] + ": " + value + supplement;
    }
}
