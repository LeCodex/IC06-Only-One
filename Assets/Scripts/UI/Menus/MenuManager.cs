using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class MenuManager : MonoBehaviour
{
    public Transform options;
    public int currentOption;

    int maxOptions;
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
        UpdateMenu();
    }

    protected void UpdateMenu()
    {
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

	protected void UpdateOptionColor(Color c)
	{
        options.GetChild(currentOption).GetComponent<Text>().color = c;
    }

    protected abstract void ChooseCurrentOption();
}
