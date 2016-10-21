using UnityEngine;
using System.Collections;

public class Terminal : Interactable
{

    private StateContoller m_SC;
    private const short OFF = 1;
    private const short ON = 2;
    private const short HACKED = 4;

    public override void interact()
    {
        --ExitManager.m_numTerminalsLeft;
        m_SC.transition(HACKED);
    }

    // Use this for initialization
    void Start ()
    {
        m_SC = new StateContoller(this);
        m_SC.transition(OFF);
    }

    [Transition(StateContoller.ANY_STATE, OFF)]
    private void anyToOff()
    {
        m_canInteract = false;
    }
    [Transition(StateContoller.ANY_STATE, ON)]
    private void anyToOn()
    {
        m_canInteract = true;
    }
    [Transition(StateContoller.ANY_STATE, HACKED)]
    private void anyToHacked()
    {

    }
    [Update(HACKED)]
    private void hacked()
    { }
    [Update(OFF)]
    private void off()
    { }
    [Update(ON)]
    private void on()
    { }

    // Update is called once per frame
    void Update ()
    {
        m_SC.update();

    }
}
