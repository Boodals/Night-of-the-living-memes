using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    private  SpawnPoint[] m_spawnPoints;
    
    void Start ()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag(Tags.SpawnPoint);
        m_spawnPoints = new SpawnPoint[spawns.Length];

        for(int i=0; i < spawns.Length; ++i)
        {
            m_spawnPoints[i] = spawns[i].GetComponent<SpawnPoint>();
            m_spawnPoints[i].spawn();
        }
    }
}
