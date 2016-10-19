using UnityEngine;
using System.Collections;

public class TVManz : EnemyBase
{
    public NavMesh m_mesh;
    private NavMeshAgent m_navAgent;

    //guna do my fancy state system for this later

	// Use this for initialization
	void Start ()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
