using UnityEngine;
using System.Collections;
public class ExitDoor : InteractiveMono
{
    public Transform m_playerStartTransform;
    public Light m_activeLight;

    public StateContoller m_SC;
    private PlayerScript m_player;
    private float m_timer;

    public const short INACTIVE = 1;
    public const short ENTER_LEVEL = 2;
    public const short EXIT_LEVEL = 4;
    public const short ACTIVE = 8;
    public const short HACKABLE = 16;

    public GameObject lightModel;
    public Material lightMaterial;

    public GameObject[] doors;
    public Vector3[] offsets;

    public bool opening = false;

    // Use this for initialization
    void Start ()
    {
        
    }

    public override void init()
    {
        base.init();
        m_SC = new StateContoller(this);
        m_SC.transition(INACTIVE);

        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
        m_activeLight = GetComponentInChildren<Light>();
        m_canInteract = false;

        lightMaterial = lightModel.GetComponent<MeshRenderer>().materials[1];
    }
    public override void interact()
    {
        base.interact();
        GameManager.levelUp();
        opening = true;
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
    [Update(HACKABLE)]
    private void hackable()
    {
        //do nothin' probably?
    }
    [Update(ENTER_LEVEL)]
    private void enter()
    {

    }
    [Update(EXIT_LEVEL)]
    private void exit()
    {
        //m_fader.color = Color.Lerp(m_startCol, m_fadeTarget, m_timer/ m_fadeDuration);
        //if (m_timer >= m_fadeDuration + m_fadeLinger)
        //{
        //    ExitManager.reset(this);
        //    m_SC.transition(ENTER_LEVEL);
        //}
    }

    [Transition(StateContoller.ANY_STATE, INACTIVE)]
    private void anyToInactive()
    {
        //close doors
        m_activeLight.enabled = false;//fade out would look nicer
        m_canInteract = false;
    }
    [Transition(StateContoller.ANY_STATE, ACTIVE)]
    private void anyToActive()
    {
        if (m_activeLight != null)
        {
            m_activeLight.enabled = true;
            m_activeLight.color = Color.green;
            m_canInteract = true;
        }

        
        HUDScript.HUDsingleton.ToggleExitNotif(true);
    }
    [Transition(StateContoller.ANY_STATE, HACKABLE)]
    private void anyToHackable()
    {
        m_activeLight.enabled = true;
        m_activeLight.color = Color.red;
        m_canInteract = false;
    }
    [Transition(StateContoller.ANY_STATE, ENTER_LEVEL)]
    private void anyToEnter()
    {
        GameManager.transformPlayer(m_playerStartTransform);
        //open the doors?

        //auto pilot the player forwards a few steps?

        //start fade-to-white?
        m_timer = 0.0f;
        m_canInteract = false;

        opening = true;
       // GameManager.gameManagerSingleton.m_exitManager.deactivateCurDoor(this);
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
        m_canInteract = false;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        m_SC.update();
        m_timer += Time.deltaTime;

        if(opening)
        {
            for(int i=0; i<doors.Length; i++)
            {
                doors[i].transform.localPosition = Vector3.Lerp(doors[i].transform.localPosition, offsets[i], 0.99f * Time.deltaTime);
            }
        }

        UpdateLightMaterial();
    }

    void UpdateLightMaterial()
    {
        lightMaterial.SetColor("_EmissionColor", m_activeLight.color);
    }
}
