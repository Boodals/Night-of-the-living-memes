using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

    public Transform[] pickupLocations;

    public GameObject ammoBoxPrefab;

    public List<int> usedValues;

    int amountOfPickupsToSpawn;
    public int pickupsSpawned;
    int randomIndex;

    bool spawnObject;

    // Use this for initialization
    void Start () {
        usedValues = new List<int>();
        pickupsSpawned = 0;
        amountOfPickupsToSpawn = 5;
        spawnObject = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (pickupsSpawned < amountOfPickupsToSpawn)
        {
            randomIndex = Random.Range(0, 10);
            spawnObject = true;

            for (int i = 0; i != usedValues.Count; i++)
            {
                if (randomIndex == usedValues[i])
                {
                    spawnObject = false;
                }
            }
            
            if (spawnObject == true)
            {
                usedValues.Add(randomIndex);
                Instantiate(ammoBoxPrefab, pickupLocations[randomIndex].position, pickupLocations[randomIndex].rotation);
                pickupsSpawned++;
            }

        }

    }
}
