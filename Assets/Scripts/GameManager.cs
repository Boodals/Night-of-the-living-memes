using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManagerSingleton;

    public enum GameStates { TITLESCREEN, PLAYING, FINISHINGLEVEL, GAMEOVER, LEADERBOARD }
    public GameStates currentGameState;

    public static int currentStage = 0;

    public float s_scoreStageModifier;
    public static float s_score;
    private float m_internalScore;

    // Use this for initialization
    void Awake()
    {
        if (!gameManagerSingleton)
        {
            gameManagerSingleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {


        m_internalScore += Time.deltaTime * ((currentStage + 1) * s_scoreStageModifier);
        s_score = (int)m_internalScore;
    }

    public void ChangeGameState(GameStates newGameState)
    {
        Debug.Log("Game state is now " + newGameState + "!");
        currentGameState = newGameState;
    }

    public GameStates GetCurrentGameState()
    {
        return currentGameState;
    }

    public bool IsCurrentGameState(GameStates isItThis)
    {
        return currentGameState == isItThis;
    }
}
