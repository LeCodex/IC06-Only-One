using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameRoundState;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    static public GameManager current;
    public Transform playersGroup;
    public List<PlayerScript> players; // The Game manager takes care of all the players (creation, access)
    public Animator intermissionTransition;
    public Transform intermissionHuds;
    public GameObject playHUD;
    public Transform playerHUDs;

    public int currentArenaScene { private set; get; } = 1;
    public AsyncOperation sceneLoading { private set; get; }

    [SerializeField]
    RoundState currentState;
    RoundStateBase state;
    Dictionary<RoundState, RoundStateBase> roundStates = new Dictionary<RoundState, RoundStateBase>()
    {
        { RoundState.Selection, new RoundStateSelection() },
        { RoundState.Play, new RoundStatePlay() },
        { RoundState.Intermission, new RoundStateIntermission() }
    };

    void Awake()
    {
        current = this;
        state = roundStates[currentState];
        players = new List<PlayerScript>(playersGroup.GetComponentsInChildren<PlayerScript>());

        foreach (PlayerScript player in players)
        {
            FindObjectOfType<CinemachineTargetGroup>().AddMember(player.GetComponent<Transform>(), 1, 5);
            intermissionHuds.GetChild(player.id).gameObject.SetActive(true);
        }
    }

    void Update()
    {
        state.Update();
    }

    public void ChangeState(RoundState newState, float delay = 0f)
	{
        if (currentState == newState) return;

        if (delay == 0f)
		{
            state.ExitState();
            currentState = newState;
            state = roundStates[newState];
            state.EnterState();
        } 
        else
		{
            StartCoroutine(ChangeStateIn(newState, delay));
		}
	}

    IEnumerator ChangeStateIn(RoundState newState, float delay)
	{
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
	}

    public void LoadNextLevel()
	{
        StartCoroutine(CoLoadNextLevel());
    }

    IEnumerator CoLoadNextLevel()
	{
        yield return new WaitForSeconds(2f);

        if (currentArenaScene > 0) SceneManager.UnloadSceneAsync(currentArenaScene);

        int nextLevelIndex = 1;
        currentArenaScene = nextLevelIndex;
        sceneLoading = SceneManager.LoadSceneAsync(nextLevelIndex, LoadSceneMode.Additive);
    }
}
