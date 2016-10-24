using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManagerSingleton;
    private PlayerScript m_player;

    public Fader m_fader;

    public enum GameStates { TITLESCREEN, PLAYING, FINISHINGLEVEL, GAMEOVER, LEADERBOARD }
    public GameStates currentGameState;

    public static int currentStage = 0;

    public float m_scoreStageModifier;
    public static int s_score;
    private static float m_internalScore;
    public ExitManager m_exitManager;
    public EnemyManager m_enemyManager;
   

    float secondTimer;
    int seconds = 0, minutes = 0;

    // Use this for initialization
    void Awake()
    {
        if (!gameManagerSingleton)
        {
            gameManagerSingleton = this;
            DontDestroyOnLoad(gameObject);

            init();
            m_fader.setStartColour(Color.black);
            m_fader.fade(3.0f, Color.clear, 0,0,null);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void transformPlayer(Transform _target)
    {
        gameManagerSingleton.m_player.transform.position = _target.position;
        gameManagerSingleton.m_player.transform.rotation = _target.rotation;
    }
 

    // Update is called once per frame
    void Update()
    {
        m_internalScore += Time.deltaTime * m_scoreStageModifier * (currentStage + 1);
        s_score = (int)m_internalScore;
        HUDScript.HUDsingleton.UpdateScore(s_score);
        PersistentScript.s_score = s_score;

        HandleTimer();
    }

    private void init()
    {
        Debug.Log("init");
        if (!IsCurrentGameState(GameStates.GAMEOVER))
        {
            m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
            m_enemyManager = GameObject.Find("Enemy stuff").GetComponent<EnemyManager>();
            m_exitManager = GameObject.Find("DoorAndTerminal").GetComponent<ExitManager>();
            m_exitManager.init();

        }
    }
    
    public void OnLevelWasLoaded()
    {
        if (currentStage > 0)
        {
            m_exitManager.spawnPlayerAtRandomDoor(m_player);
            gameManagerSingleton.m_fader.setStartColour(Color.white, true);
            gameManagerSingleton.m_fader.fade(1.25f, Color.clear, 0.0f, 0.5f, null);
        }
    }
    public static void levelUp()
    {
        ++currentStage;
        //stop enemies?
        gameManagerSingleton.m_fader.setStartColour(Color.clear);
        gameManagerSingleton.m_fader.fade(1.25f, Color.white, 0.0f, 0.0f, changeLevel);
    }
    private static void changeLevel()
    {
        //SceneManager.UnloadScene("asylumEditor");
        SceneManager.LoadScene("asylumEditor");
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

    void HandleTimer()
    {
        secondTimer += Time.deltaTime;

        while(secondTimer>1)
        {
            secondTimer -= 1;
            seconds += 1;
        }

        while(seconds>59)
        {
            seconds -= 60;
            minutes += 1;
        }

        PhoneHUDScript.phoneHUDSingleton.UpdateTimer(minutes, seconds);
    }
}
