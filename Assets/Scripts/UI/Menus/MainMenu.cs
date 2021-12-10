using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuManager
{
    public OptionsMenu optionsMenu;
    public GameObject credits;
    public Fade fadeToBlack;

    protected override void ChooseCurrentOption()
    {
        // Debug.Log("Chose " + currentOption);
        switch(currentOption)
		{
            case 0:
                StartCoroutine(LoadGameScene());
                break;

            case 1:
                enabled = false;
                optionsMenu.gameObject.SetActive(true);
                options.gameObject.SetActive(false);
                break;

            case 2:
                options.gameObject.SetActive(false);
                credits.SetActive(true);
                currentOption = 4;
                break;

            case 3:
                Application.Quit();
                break;

            case 4:
                options.gameObject.SetActive(true);
                credits.SetActive(false);
                currentOption = 2;
                break;
		}
    }

    IEnumerator LoadGameScene()
	{
        fadeToBlack.goalFade = 1f;

        yield return new WaitForSeconds(.6f);

        SceneManager.LoadSceneAsync(1);
	}
}
