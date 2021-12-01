using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Transform options;
    public GameObject credits;
    public Fade fadeToBlack;

    int maxOptions;
    int currentOption;
    float timeSinceLastInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        maxOptions = options.childCount;
        currentOption = 0;
        UpdateOptionColor(new Color(0, 255, 0, 255));
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastInput -= Time.deltaTime;

        float axis = Input.GetAxisRaw("Vertical1");
        // Debug.Log(axis);

        if (currentOption < maxOptions)
		{
            if (Math.Abs(axis) < .5f)
            {
                timeSinceLastInput = 0f;
            }
            else if (timeSinceLastInput <= 0f)
            {
                timeSinceLastInput = .5f;
                UpdateOptionColor(new Color(255, 255, 255, 255));

                currentOption = (currentOption - (int)Math.Round(axis) + maxOptions) % maxOptions;
                UpdateOptionColor(new Color(0, 255, 0, 255));
            }
        }

        if (Input.GetButtonDown("Attack1")) ChooseCurrentOption();
    }

	void UpdateOptionColor(Color c)
	{
        options.GetChild(currentOption).GetComponent<Text>().color = c;
    }

    void ChooseCurrentOption()
    {
        // Debug.Log("Chose " + currentOption);
        switch(currentOption)
		{
            case 0:
                StartCoroutine(LoadGameScene());
                break;

            case 1:

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
