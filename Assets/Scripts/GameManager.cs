using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManagerSingleton;
    public EnemyManager m_enemyManager;
    public Transform m_spawn;
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

            m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
            m_fader = GameObject.Find("FadeCanvas").GetComponent<Canvas>().GetComponentInChildren<Image>();

            m_startCol = m_fader.color;
            m_fadeState = FADE.IN;
            m_player.transform.position = m_spawn.position;
            m_timer = 0.0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void fadeOut()
    {
        Debug.Log("fading out!");
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
        Debug.Log("fading in!");
        m_fader.color = Color.Lerp(gameManagerSingleton.m_fadeTarget, m_startCol, m_timer / gameManagerSingleton.m_fadeDuration);
        if (m_timer >= gameManagerSingleton.m_fadeDuration)
        {
            m_fadeState = FADE.NONE;
            m_timer = 0.0f;
        }
    }

    public static void setSpawnPoint(Transform _spawn)
    {
        gameManagerSingleton.m_spawn.position = _spawn.position;
        gameManagerSingleton.m_spawn.rotation = _spawn.rotation;
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

    public static void levelUp()
    {
        ++currentStage;
        m_fadeState = FADE.OUT;
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
