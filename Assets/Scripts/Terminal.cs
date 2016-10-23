using UnityEngine;
using System.Collections;

public class Terminal : InteractiveMono
{

    public StateContoller m_SC;
    public const short OFF = 1;
    public const short ON = 2;
    public const short HACKED = 4;

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
    void Start ()
    {
    
    }

    [Transition(StateContoller.ANY_STATE, OFF)]
    private void anyToOff()
    {
        //TURN OFF MEME

        m_canInteract = false;
    }
    [Transition(StateContoller.ANY_STATE, ON)]
    private void anyToOn()
    {
        //TURN ON MEME

        m_canInteract = true;
    }
    [Transition(StateContoller.ANY_STATE, HACKED)]
    private void anyToHacked()
    {
        //SHOCKED EFFECT ON TERMINAL?

        m_canInteract = false;
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
