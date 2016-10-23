using UnityEngine;
using System.Collections;

public class NyanCatSpawner : MonoBehaviour
{

	public float timeToSpawn = 180f; //3 mins

	public NyanCat nyanCat;


	void Start()
	{
		nyanCat.gameObject.SetActive(false);
		StartCoroutine(WaitForSpawn());
	}


	IEnumerator WaitForSpawn()
	{
		yield return new WaitForSeconds(timeToSpawn);

		Debug.Log("Spawned nyan cat");
		nyanCat.gameObject.SetActive(true);
	}


}
