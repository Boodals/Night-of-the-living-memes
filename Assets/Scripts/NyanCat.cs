using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NyanCat : MonoBehaviour
{

	public Transform targetTrans;

	private void Update()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		
		agent.destination = targetTrans.position;
	}

}
