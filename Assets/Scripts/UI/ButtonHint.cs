using PerkSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHint : MonoBehaviour
{
    public Canvas hintCanvas;
	public float hintDuration; // 0 means inifnite

	float timeRemaining;

	private void Update()
	{
		if (timeRemaining > 0f)
		{
			timeRemaining -= Time.deltaTime;
		}

		if (timeRemaining < 0f)
		{
			timeRemaining = 0f;
			HideHint();
		}
	}

	public void ShowHint(string hint, Sprite button)
	{
		hintCanvas.enabled = true;
		hintCanvas.transform.Find("Hint Text").GetComponent<Text>().text = hint;
		hintCanvas.transform.Find("Hint Button").GetComponent<Image>().sprite = button;

		timeRemaining = hintDuration;
	}

	public void HideHint()
	{
		hintCanvas.enabled = false;
	}
}
