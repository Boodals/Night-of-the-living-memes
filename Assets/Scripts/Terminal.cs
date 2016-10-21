using UnityEngine;
using System.Collections;

public class Terminal : Interactable
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
    }

    // Use this for initialization
    void Start ()
    {
    
    }

    [Transition(StateContoller.ANY_STATE, OFF)]
    private void anyToOff()
    {
        //Debug.Log("trans to off");
        GetComponent<MeshRenderer>().material.color = Color.grey;//rofl unity has english spelling
        m_canInteract = false;
    }
    [Transition(StateContoller.ANY_STATE, ON)]
    private void anyToOn()
    {
        //Debug.Log("trans to on");
        m_canInteract = true;
        GetComponent<MeshRenderer>().material.color = Color.red;//rofl unity has english spelling
    }
    [Transition(StateContoller.ANY_STATE, HACKED)]
    private void anyToHacked()
    {
        //Debug.Log("trans to hacked");
        GetComponent<MeshRenderer>().material.color = Color.green;
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
    { }

    // Update is called once per frame
    void Update ()
    {
        m_SC.update();
    }
}
