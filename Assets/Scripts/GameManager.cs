using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManagerSingleton;
    private PlayerScript m_player;

    public float m_fadeDuration;
    public float m_fadeLinger;
    public Color m_fadeTarget;
    private static Image m_fader;
    private static Color m_startCol;
    private static float m_timer;

    public enum GameStates { TITLESCREEN, PLAYING, FINISHINGLEVEL, GAMEOVER, LEADERBOARD }
    public GameStates currentGameState;

    public static int currentStage = 0;

    public static float s_scoreStageModifier;
    public static float s_score;
    private static float m_internalScore;
    public ExitManager m_exitManager;
    public EnemyManager m_enemyManager;
    private enum FADE
    {
        OUT,
        IN,
        NONE
    }
    private static FADE m_fadeState;
    // Use this for initialization
    void Awake()
    {
        if (!gameManagerSingleton)
        {
            gameManagerSingleton = this;
            DontDestroyOnLoad(gameObject);

            init();
            m_startCol = m_fader.color;
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
    public static void fadeOut()
    {
        m_fader.color = Color.Lerp(m_startCol, gameManagerSingleton.m_fadeTarget, m_timer / gameManagerSingleton.m_fadeDuration);
        if (m_timer >= gameManagerSingleton.m_fadeDuration + gameManagerSingleton.m_fadeLinger)
        {
            //done
            m_fadeState = FADE.IN;
            m_timer = 0.0f;
        }
    }

    public static void fadeIn()
    {
        m_fader.color = Color.Lerp(gameManagerSingleton.m_fadeTarget, m_startCol, m_timer / gameManagerSingleton.m_fadeDuration);
        if (m_timer >= gameManagerSingleton.m_fadeDuration)
        {
            m_fadeState = FADE.NONE;
            m_timer = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
     
        switch (m_fadeState)
        {
            case FADE.OUT:
                fadeOut();
                break;
            case FADE.IN:
                fadeIn();
                break;
            case FADE.NONE:
                m_internalScore += Time.deltaTime * ((currentStage + 1) * s_scoreStageModifier);
                s_score = (int)m_internalScore;
                break;
            default:
                break;
        }
    }

    private void init()
    {
        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
        m_fader = GameObject.Find("FadeCanvas").GetComponent<Canvas>().GetComponentInChildren<Image>();
        m_enemyManager = GameObject.Find("Enemy stuff").GetComponent<EnemyManager>();
        m_exitManager = GameObject.Find("DoorAndTerminal").GetComponent<ExitManager>();
        m_exitManager.init();

        m_fadeState = FADE.IN;
        m_timer = 0.0f;
    }
    
    public void OnLevelWasLoaded()
    {
        init();
        m_exitManager.spawnPlayerAtRandomDoor(m_player);
    }
    public static void levelUp()
    {
        ++currentStage;
        m_fadeState = FADE.OUT;

        SceneManager.UnloadScene("asylumEditor");
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
}
