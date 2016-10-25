using UnityEngine;
using System.Collections;

public class Terminal : InteractiveMono
{

    public StateContoller m_SC;
    public const short OFF = 1;
    public const short ON = 2;
    public const short HACKED = 4;

    public Material myScreen;
    public Renderer myRenderer;
    public Light myLight;

    public Texture textureOff, textureAvailable, textureHacked;

    public AudioClip hackSound;
    AudioSource snd;

    public AudioSource vapourWave;

    public override void interact()
    {
        GameManager.gameManagerSingleton.m_exitManager.hackedATerminal();
        m_SC.transition(HACKED);
    }

    public override void init()
    {
        base.init();
        m_SC = new StateContoller(this);
        m_SC.transition(OFF);
        m_canInteract = false;       
        
    }

    // Use this for initialization
    void Awake ()
    {
        if (myRenderer)
            myScreen = myRenderer.materials[0];
        else
            Debug.Break();
        //snd = GetComponent<AudioSource>();
    }

    [Transition(StateContoller.ANY_STATE, OFF)]
    private void anyToOff()
    {
        //TURN OFF MEME
        myScreen.SetTexture("_MainTex", textureOff);
        m_canInteract = false;
        myLight.enabled = false;
        vapourWave.Stop();
    }
    [Transition(StateContoller.ANY_STATE, ON)]
    private void anyToOn()
    {
        //TURN ON MEME
        myScreen.SetTexture("_MainTex", textureAvailable);
        m_canInteract = true;
        myLight.enabled = true;
        vapourWave.Play();
    }
    [Transition(StateContoller.ANY_STATE, HACKED)]
    private void anyToHacked()
    {
        //SHOCKED EFFECT ON TERMINAL?
        myScreen.SetTexture("_MainTex", textureHacked);

        GetComponent<AudioSource>().PlayOneShot(hackSound);
        m_canInteract = false;
        myLight.enabled = false;

        vapourWave.Stop();
    }

    [Update(HACKED)]
    private void hacked()
    { }
    [Update(OFF)]
    private void off()
    {
    }
    [Update(ON)]
    private void on()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        m_SC.update();
    }
}
