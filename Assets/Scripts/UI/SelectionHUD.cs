using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SelectionHUD : MonoBehaviour
{
    public GameObject joinedHud;
    public GameObject notJoinedHud;
    public Image readySprite;
    public PlayerScript player;
    public PlayerSelection parent;
    public SpriteAtlas[] skins;
    public string[] skinNames;

    public bool ready { private set; get; } = false;

    Image image;
    Text skinName;
    int skinSelection;
    float waitBeforeInput;

	private void Awake()
	{
        image = joinedHud.GetComponentInChildren<Image>();
        skinName = joinedHud.GetComponentInChildren<Text>();
        parent = GetComponentInParent<PlayerSelection>();
	}

	// Start is called before the first frame update
	void Start()
    {
        player = GameManager.current.players[transform.GetSiblingIndex() - 1];
        skinSelection = transform.GetSiblingIndex() - 1;
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        waitBeforeInput -= Time.deltaTime;
        
        if (joinedHud)
		{
            if (Input.GetButtonDown("Attack" + player.id))
			{
                ready = !ready;
                readySprite.enabled = ready;
            }

            if (!ready)
			{
                float axis = Input.GetAxisRaw("Horizontal" + player.id);
                if (Math.Abs(axis) < 1f)
                {
                    waitBeforeInput = 0f;
                }
                else if (waitBeforeInput <= 0f)
                {
                    waitBeforeInput = .5f;
                    skinSelection = (skinSelection + (int)Math.Floor(axis) + skins.Length) % skins.Length;
                    UpdateDisplay();
                }
            }

            if (Input.GetButtonDown("Secondary" + player.id))
            {
                Leave();
            }
        }
    }

	public void Join(int playerID)
	{
        player.id = playerID;
        ready = false;
        readySprite.enabled = ready;
        joinedHud.SetActive(true);
        notJoinedHud.SetActive(false);
	}

    void UpdateDisplay()
	{
        Sprite sprite = skins[skinSelection].GetSprite("dr0");

        image.sprite = sprite;
        skinName.text = skinNames[skinSelection];

        player.controller.aliveAnimator.GetComponent<CharacterAnimator>().spriteSheet = skins[skinSelection];
        player.playerHud.portrait.sprite = sprite;
        player.intermissionHud.portrait.sprite = sprite;
    }

    public void Leave(bool dontCallParent = false)
	{
        player.ready = false;
        joinedHud.SetActive(false);
        notJoinedHud.SetActive(true);
        // if (!dontCallParent) parent.MoveHudsBack();
    }
}
