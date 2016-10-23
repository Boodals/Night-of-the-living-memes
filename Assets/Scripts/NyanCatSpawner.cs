using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NyanCatSpawner : MonoBehaviour
{

	public float timeToSpawn = 180f; //3 mins

	public NyanCat nyanCat;

	public PlayerScript player;


	void Start()
	{
		nyanCat.gameObject.SetActive(false);
		StartCoroutine(WaitForSpawn());
	}


	IEnumerator WaitForSpawn()
	{
		yield return new WaitForSeconds(timeToSpawn);
		SpawnNyanCat();
	}


	private void SpawnNyanCat()
	{
		NyanCatSpawnPoint bestSpawn = null;
		float bestDist = Mathf.NegativeInfinity;

		foreach(NyanCatSpawnPoint sp in FindObjectsOfType<NyanCatSpawnPoint>())
		{
			float dist = (sp.transform.position - player.transform.position).sqrMagnitude;
            if(bestDist < dist)
			{
				bestDist = dist;
				bestSpawn = sp;
			}
		}

		nyanCat.transform.position = bestSpawn.transform.position;
		nyanCat.transform.rotation = bestSpawn.transform.rotation;

		nyanCat.gameObject.SetActive(true);

		Debug.Log("Spawned nyan cat");
	}


}
