﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NyanCat : MonoBehaviour
{

	public Transform targetTrans;

	public PhoneHUDScript phone;

	public float speedPerLevel = 0.5f;


	public AudioClip spawnOneShot;
	public AudioClip killedPlayerOneShot;

	public AudioSource oneShotSource;


	private void Awake()
	{
		phone.distance = Mathf.Infinity;
	}

	private void Start()
	{
		GetComponent<NavMeshAgent>().speed += speedPerLevel * GameManager.currentStage;

		if (spawnOneShot != null)
		{
			oneShotSource.PlayOneShot(spawnOneShot);
		}
    }

	private void Update()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();

		agent.destination = targetTrans.position;


		phone.distance = Vector3.Distance(targetTrans.position, transform.position);
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerScript ps = other.GetComponent<PlayerScript>();

			if(ps != null)
			{
				ps.StartDying();

				if (killedPlayerOneShot != null)
				{
					oneShotSource.PlayOneShot(killedPlayerOneShot);
				}
			}
		}
	}
}
