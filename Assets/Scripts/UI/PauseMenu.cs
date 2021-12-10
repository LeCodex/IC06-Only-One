using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WeaponSystem;

public class PauseMenu : MonoBehaviour
{
    public Transform options;
    public GameObject menu;
    public Fade fadeToBlack;

    public bool shown { private set; get; }

    int maxOptions;
    int currentOption;
    float timeSinceLastInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        maxOptions = options.childCount;
        currentOption = 0;
        UpdateOptionColor(new Color(0, 255, 0, 255));

        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shown) return;

        float axis = Input.GetAxisRaw("Vertical1");
        // Debug.Log(axis);

        if (currentOption < maxOptions)
        {
            if (Math.Abs(axis) < .5f)
            {
                timeSinceLastInput = Time.unscaledTime;
            }
            else if (timeSinceLastInput <= Time.unscaledTime)
            {
                timeSinceLastInput = Time.unscaledTime + .5f;
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
        switch (currentOption)
        {
            case 0:
                Hide();
                break;

            case 1:

                break;

            case 2:
                StartCoroutine(LoadMenuScene());
                Hide();
                break;
        }
    }

    IEnumerator LoadMenuScene()
    {
        fadeToBlack.goalFade = 1f;
        GameManager.current.EndSlowDown();

        yield return new WaitForSeconds(.6f);

        SceneManager.LoadSceneAsync(0);
    }

    public void Show()
	{
        menu.SetActive(true);
        GameManager.current.SlowDownTime(1f, 99999999999f);
        foreach (Weapon w in FindObjectsOfType<Weapon>()) w.enabled = false;
        shown = true;
    }

    public void Hide()
	{
        menu.SetActive(false);
        GameManager.current.EndSlowDown();
        foreach (Weapon w in FindObjectsOfType<Weapon>()) w.enabled = true;
        shown = false;
    }
}
