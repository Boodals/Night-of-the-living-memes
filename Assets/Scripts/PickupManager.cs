using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

    GameObject[] pickupObjects;
    public Transform[] pickupLocations;

    public GameObject ammoBoxPrefab;

    public List<int> usedValues;

    public int amountOfPickupsToSpawn;
    public int pickupsSpawned;
    int randomIndex;

    bool spawnObject;

    // Use this for initialization
    void Start () {
        pickupLocations = new Transform[30];
        usedValues = new List<int>();

        pickupObjects = GameObject.FindGameObjectsWithTag("AmmoSpawn");

        for (int i = 0; i < pickupObjects.Length; i++)
        {
            pickupLocations[i] = pickupObjects[i].transform;
        }

        switch(GameManager.currentStage)
        {
            case 0:
                amountOfPickupsToSpawn = 5;
                break;
            case 1:
                amountOfPickupsToSpawn = 3;
                break;
            default:
                amountOfPickupsToSpawn = 2;
                break;
        }
       
        pickupsSpawned = 0;    
        spawnObject = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (pickupsSpawned < amountOfPickupsToSpawn)
        {
            randomIndex = Random.Range(0, 29);
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
