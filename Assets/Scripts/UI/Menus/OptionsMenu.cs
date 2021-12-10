using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MenuManager
{
    public MainMenu mainMenu;
    public Slider volumeBar;

    protected override void ChooseCurrentOption()
    {
        // Debug.Log("Chose " + currentOption);
        switch(currentOption)
		{
            case 0:
                AudioListener.volume -= .1f;
                if (AudioListener.volume < 0f) AudioListener.volume = 1f;

                volumeBar.value = AudioListener.volume;
                break;

            case 1:
                gameObject.SetActive(false);
                mainMenu.options.gameObject.SetActive(true);
                mainMenu.enabled = true;

                UpdateOptionColor(new Color(255, 255, 255, 255));
                currentOption = 0;
                UpdateOptionColor(new Color(0, 255, 0, 255));
                break;
		}
    }
}
