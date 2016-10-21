using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour
{
    public ExitDoor[] m_doors;
    private static ExitManager s_instance;
    private int m_curActive;
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
    }

    public static void activateRandomDoor()
    {
        s_instance.m_curActive = Random.Range(0, s_instance.m_doors.Length);
        s_instance.m_doors[s_instance.m_curActive].activate();
    }

    public static void deactivateCurDoor()
    {
        if (s_instance.m_curActive >= 0) s_instance.m_doors[s_instance.m_curActive].deactivate();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            deactivateCurDoor();
            activateRandomDoor();
        }
           
    }
}
