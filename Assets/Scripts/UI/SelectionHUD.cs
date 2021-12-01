using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionHUD : MonoBehaviour
{
    public GameObject joinedHud;
    public GameObject notJoinedHud;
    public Image readySprite;
    public PlayerScript player;
    public PlayerSelection parent;

    public bool ready { private set; get; } = false;

    Image image;

	private void Awake()
	{
        image = joinedHud.GetComponentInChildren<Image>();
        parent = GetComponentInParent<PlayerSelection>();
	}

	// Start is called before the first frame update
	void Start()
    {
        player = GameManager.current.players[transform.GetSiblingIndex() - 1];
    }

    // Update is called once per frame
    void Update()
    {
        player.render.sprite = image.sprite;

        if (joinedHud)
		{
            if (Input.GetButtonDown("Attack" + player.id))
			{
                ready = !ready;
                readySprite.enabled = ready;
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

    public void Leave(bool dontCallParent = false)
	{
        player.ready = false;
        joinedHud.SetActive(false);
        notJoinedHud.SetActive(true);
        // if (!dontCallParent) parent.MoveHudsBack();
    }
}
