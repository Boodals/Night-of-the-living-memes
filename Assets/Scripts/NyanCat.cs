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

	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerScript ps = other.GetComponent<PlayerScript>();

			if(ps != null)
			{
				ps.StartDying();
			}
		}
	}
}
