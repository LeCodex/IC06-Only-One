using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WeaponSystem;

public class PauseMenu : MenuManager
{
    public GameObject menu;
    public Fade fadeToBlack;

    public bool shown { private set; get; }

    // Start is called before the first frame update
    void Awake()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shown) return;
        UpdateMenu();
    }

    protected override void ChooseCurrentOption()
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
