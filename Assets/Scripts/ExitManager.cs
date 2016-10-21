using UnityEngine;
using System.Collections.Generic;

public class ExitManager : MonoBehaviour
{
    public int[] m_terminalsRequiredPerLevel;

    private static List<ExitDoor> m_doors;
    private static List<Terminal> m_terminals;

    private static ExitManager s_instance;
    private static ExitDoor m_curDoor;
    private static int m_numTerminalsLeft;
    // Use this for initialization
    void Start()
    {
        Debug.Assert(s_instance == null, "Only one ExitManger allowed!");
        s_instance = this;

        GameObject[] doors = GameObject.FindGameObjectsWithTag(Tags.Door);
        m_doors = new List<ExitDoor>(doors.Length);
        for (int i = 0; i < doors.Length; ++i)
        {
            m_doors.Add(doors[i].GetComponent<ExitDoor>());
        }
        m_curDoor = null;

        GameObject[] terminals = GameObject.FindGameObjectsWithTag(Tags.Terminal);
        m_terminals = new List<Terminal>(terminals.Length);
        for (int i = 0; i < terminals.Length; ++i)
        {
            m_terminals.Add(terminals[i].GetComponent<Terminal>());
            m_terminals[i].init();
        }

        reset();
    }
    public static void reset()
    {
        foreach (Terminal t in m_terminals)
        {
            t.m_SC.transition(Terminal.OFF);
        }

        if (GameManager.currentStage < s_instance.m_terminalsRequiredPerLevel.Length)
        {
            m_numTerminalsLeft = s_instance.m_terminalsRequiredPerLevel[GameManager.currentStage];
            List<Terminal> unused = new List<Terminal>();
            foreach (Terminal t in m_terminals)
            {
                unused.Add(t);
            }
            //for each terminal we need...
            for (int i = 0; i < m_numTerminalsLeft; ++i)
            {
                //get random index in unsude list
                int index = Random.Range(0, unused.Count);
                //turn on terminal at that index
                unused[index].m_SC.transition(Terminal.ON);
                //remove from unused list
                unused.Remove(unused[index]);
            }
        }
        //no need to do slow list shinanigens if we're using all the terminals...
        else
        {
            m_numTerminalsLeft = m_terminals.Count;
            for (int i = 0; i < m_numTerminalsLeft; ++i)
            {
                m_terminals[i].m_SC.transition(Terminal.ON);
            }
        }

        //really this should activate it but in waiting mode, needs to be hacked first
        activateRandomDoor();
    }
    public static void activateRandomDoor()
    {
        int index = Random.Range(0, m_doors.Count);
        m_doors[index].m_SC.transition(ExitDoor.HACKABLE);
        m_curDoor = m_doors[index];
        m_doors.Remove(m_curDoor);
    }

    public static void deactivateCurDoor(ExitDoor _door)
    {
        if (m_curDoor!=null)
        {
            _door.m_SC.transition(ExitDoor.INACTIVE);
            m_doors.Add(_door);
        }
    }

    public static void hackedATerminal()
    {
        if (--m_numTerminalsLeft <= 0)
        {
            m_curDoor.m_SC.transition(ExitDoor.ACTIVE);
        }

    }
    void Update()
    {
   
    }
}
