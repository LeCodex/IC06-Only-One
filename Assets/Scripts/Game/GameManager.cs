using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameRoundState;

public class GameManager : MonoBehaviour
{
    static public GameManager current;
    public List<PlayerScript> players = new List<PlayerScript>(); // The Game mamanger takes care of all the players (creation, access)
    public Animator intermissionTransition;

    public int currentArenaScene { private set; get; }
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
        SceneManager.UnloadSceneAsync(currentArenaScene);

        int nextLevelIndex = 1;
        currentArenaScene = nextLevelIndex;
        sceneLoading = SceneManager.LoadSceneAsync(nextLevelIndex);
    }
}
