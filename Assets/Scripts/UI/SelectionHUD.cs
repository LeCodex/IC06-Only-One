using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionHUD : MonoBehaviour
{
    public GameObject joinedHud;
    public GameObject notJoinedHud;
    public Image readySprite;

    public int id { private set; get; }
    
    Image image;

	private void Awake()
	{
        image = joinedHud.GetComponentInChildren<Image>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerScript player = GameManager.current.players[id - 1];
        player.render.sprite = image.sprite;

        if (joinedHud)
		{
            if (Input.GetButtonDown("Attack" + id))
			{
                player.ready = !player.ready;
                readySprite.enabled = player.ready;
            }
		}
    }

	public void Join(int playerID)
	{
        id = playerID;
        joinedHud.SetActive(true);
        notJoinedHud.SetActive(false);
	}
}
