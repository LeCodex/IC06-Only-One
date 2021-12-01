using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float goalFade = 0f;
    public float fadeSpeed = 2f;

    float fade = 0f;
    Image image;

	private void Awake()
	{
        image = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
    {
        if (fade < goalFade)
		{
            fade = Math.Min(goalFade, fade + Time.deltaTime * fadeSpeed);
		}
        else if (fade > goalFade)
        {
            fade = Math.Max(goalFade, fade - Time.deltaTime * fadeSpeed);
        }

        image.color = new Color(0f, 0f, 0f, fade);
    }
}
