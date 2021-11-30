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
    
    Image image;

	private void Awake()
	{
        player = GameManager.current.players[transform.GetSiblingIndex() - 1];
        image = joinedHud.GetComponentInChildren<Image>();
        parent = GetComponentInParent<PlayerSelection>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player.render.sprite = image.sprite;

        if (joinedHud)
		{
            if (Input.GetButtonDown("Attack" + player.id))
			{
                player.ready = !player.ready;
                readySprite.enabled = player.ready;
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
        readySprite.enabled = player.ready;
        joinedHud.SetActive(true);
        notJoinedHud.SetActive(false);
	}

    public void Leave(bool dontCallParent = false)
	{
        player.ready = false;
        joinedHud.SetActive(false);
        notJoinedHud.SetActive(true);
        if (!dontCallParent) parent.MoveHudsBack();
    }
}
