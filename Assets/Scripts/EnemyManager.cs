using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager s_inst;
    //keeping a reference to them because idk if i might need to access them some more at some point
    private static SpawnPoint[] m_spawnPoints;

    // Use this for initialization
    void Start ()
    {
        Debug.Assert(s_inst == null, "Only one enemy manager per scene!");
        s_inst = this;

        GameObject[] spawns = GameObject.FindGameObjectsWithTag(Tags.SpawnPoint);
        m_spawnPoints = new SpawnPoint[spawns.Length];

        for(int i=0; i < spawns.Length; ++i)
        {
            m_spawnPoints[i] = spawns[i].GetComponent<SpawnPoint>();
            m_spawnPoints[i].spawn();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public static void begin()
    {

    }

    public static void end()
    {

    }


}
