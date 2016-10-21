using UnityEngine;
using System.Collections.Generic;

public class ExitManager : MonoBehaviour
{
    public int[] m_terminalsRequiredPerLevel;

    private ExitDoor[] m_doors;
    private List<Terminal> m_terminals;

    private static ExitManager s_instance;
    private static int m_curActive;
    private static bool m_doorActive;
    public static int m_numTerminalsLeft;
    // Use this for initialization
    void Start()
    {
        Debug.Assert(s_instance == null, "Only one ExitManger allowed!");
        s_instance = this;

        GameObject[] doors = GameObject.FindGameObjectsWithTag(Tags.Door);
        m_doors = new ExitDoor[doors.Length];
        for (int i = 0; i < doors.Length; ++i)
        {
            m_doors[i] = doors[i].GetComponent<ExitDoor>();
        }
        m_curActive = -1;

        GameObject[] terminals = GameObject.FindGameObjectsWithTag(Tags.Terminal);
        m_terminals = new List<Terminal>(terminals.Length);
        for (int i = 0; i < terminals.Length; ++i)
        {
            m_terminals.Add(terminals[i].GetComponent<Terminal>());
            m_terminals[i].init();
        }

        reset();
        m_doorActive = false;
    }
    public static void reset()
    {
        if (GameManager.currentStage < s_instance.m_terminalsRequiredPerLevel.Length)
        {
            m_numTerminalsLeft = s_instance.m_terminalsRequiredPerLevel[GameManager.currentStage];
            List<Terminal> unused = s_instance.m_terminals;
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
            m_numTerminalsLeft = s_instance.m_terminals.Count;
            for (int i = 0; i < m_numTerminalsLeft; ++i)
            {
                s_instance.m_terminals[i].m_SC.transition(Terminal.ON);
            }
        }

        //really this should activate it but in waiting mode, needs to be hacked first
        activateRandomDoor();
    }
    public static void activateRandomDoor()
    {
        m_curActive = Random.Range(0, s_instance.m_doors.Length);
        s_instance.m_doors[m_curActive].m_SC.transition(ExitDoor.HACKABLE);
}

    public static void deactivateCurDoor()
    {
        if (m_curActive >= 0) s_instance.m_doors[m_curActive].deactivate();
    }
    void Update()
    {
        if (!m_doorActive && m_numTerminalsLeft <= 0)
        {
            m_doorActive = true;
            s_instance.m_doors[m_curActive].m_SC.transition(ExitDoor.ACTIVE);
        }
    }
}
