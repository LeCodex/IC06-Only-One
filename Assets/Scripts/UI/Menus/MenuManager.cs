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
        float axisV = Input.GetAxisRaw("VerticalMenu");
        float axisH = Input.GetAxisRaw("HorizontalMenu");
        // Debug.Log(axis);

        if (currentOption < maxOptions)
		{
            if (Math.Abs(axisV) < .5f && Math.Abs(axisH) < .5f)
            {
                timeSinceLastInput = Time.unscaledTime;
            }
            
            if (timeSinceLastInput <= Time.unscaledTime)
            {
                if (Math.Abs(axisV) > .7f)
				{
                    UpdateOptionColor(new Color(255, 255, 255, 255));

                    currentOption = (currentOption - (int)Math.Round(axisV) + maxOptions) % maxOptions;
                    UpdateOptionColor(new Color(0, 255, 0, 255));
                    timeSinceLastInput = timeSinceLastInput + .5f;
                }

                if (Math.Abs(axisH) > .7f)
				{
                    LeftRightCurrentOption(Math.Sign(axisH));
                    timeSinceLastInput = timeSinceLastInput + .3f;
                }
            }
        }

        if (Input.GetButtonDown("Confirm")) ChooseCurrentOption();
    }

	protected void UpdateOptionColor(Color c)
	{
        options.GetChild(currentOption).GetComponent<Text>().color = c;
    }

    protected abstract void ChooseCurrentOption();
    protected virtual void LeftRightCurrentOption(int axis) {}
}
