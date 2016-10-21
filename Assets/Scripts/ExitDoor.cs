using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ExitDoor : MonoBehaviour
{

    public float m_fadeDuration;
    public float m_fadeLinger;
    public Color m_fadeTarget;
    public Transform m_playerStartTransform;
    public Light m_activeLight;

    private Image m_fader;
    private Color m_startCol;
    private StateContoller m_SC;
    private PlayerScript m_player;
    private float m_timer;


    protected const short INACTIVE = 1;
    protected const short ENTER_LEVEL = 2;
    protected const short EXIT_LEVEL = 4;
    protected const short ACTIVE = 8;

    // Use this for initialization
    void Start ()
    {

        m_SC = new StateContoller(this);
        m_SC.transition(INACTIVE);

        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
        m_fader = GameObject.Find("FadeCanvas").GetComponent<Canvas>().GetComponentInChildren<Image>();

        m_startCol = m_fader.color;
    }

    public void activate()
    {
        m_SC.transition(ACTIVE);
    }

    //just for testing, as the deactivation will probably be handled internally
    public void deactivate()
    {
        m_SC.transition(INACTIVE);
    }

    [Update(INACTIVE)]
    private void inactive()
    {
        //do nothin'
    }

    [Update(ACTIVE)]
    private void active()
    {
        //do nothin' probably?
    }

    [Update(ENTER_LEVEL)]
    private void enter()
    {
        m_fader.color = Color.Lerp(m_fadeTarget,m_startCol , m_timer / m_fadeDuration);
        if (m_timer >= m_fadeDuration)
        {
            m_SC.transition(INACTIVE);
        }
    }

    [Update(EXIT_LEVEL)]
    private void exit()
    {
        m_fader.color = Color.Lerp(m_startCol, m_fadeTarget, m_timer/ m_fadeDuration);
        if (m_timer >= m_fadeDuration + m_fadeLinger)
        {
            m_SC.transition(ENTER_LEVEL);
        }
    }

    [Transition(StateContoller.ANY_STATE, INACTIVE)]
    private void anyToInactive()
    {
        //close doors
        m_activeLight.enabled = false;//fade out would look nicer
    }

    [Transition(StateContoller.ANY_STATE, ACTIVE)]
    private void anyToActive()
    {
        m_activeLight.enabled = true;
    }

    [Transition(StateContoller.ANY_STATE, ENTER_LEVEL)]
    private void anyToEnter()
    {
        //open the doors?

        //auto pilot the player forwards a few steps?

        //start fade-to-white?
        m_player.transform.position = m_playerStartTransform.position;
        //m_player.transform.transform.rotation = m_playerStartTransform.rotation;//ASK TMS ABOUT THIS
        m_timer = 0.0f;
    }
    [Transition(StateContoller.ANY_STATE, EXIT_LEVEL)]
    private void anyToExit()
    {
        //open the doors?

        //player cannot move/ is moved through doors?

        //start fade white-to-normal?

        m_timer = 0.0f;

        //reset level
        //increase score multiplier
    }

    // Update is called once per frame
    void Update ()
    {
        m_SC.update();
        m_timer += Time.deltaTime;
    }

    void OnTriggerStay(Collider _other)
    {
        if (_other.tag == Tags.Player /*&& Input.GetButtonDown("action" )*/) 
        {
            m_SC.transition(EXIT_LEVEL);
        }
    }
    
}
