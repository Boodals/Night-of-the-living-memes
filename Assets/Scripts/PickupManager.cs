using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour {

    public Transform[] pickupLocations;

    int randomIndex;
    int[] randomIndexesChosen;

    public GameObject ammoBoxPrefab;
    bool locationUsed;

    int amountOfPickupsToSpawn = 5;

	// Use this for initialization
	void Start () {
        locationUsed = false;
        randomIndexesChosen = new int[amountOfPickupsToSpawn];
        for (int i = 0; i < amountOfPickupsToSpawn; i++)
        {
            randomIndex = Random.Range(0, 10);
            for(int index = 0; index < amountOfPickupsToSpawn; index++)
            {
                if (randomIndex == randomIndexesChosen[index])
                {
                    locationUsed = true;
                }
            }
            if (locationUsed == false)
            {
                Instantiate(ammoBoxPrefab, pickupLocations[randomIndex].position, pickupLocations[randomIndex].rotation);
            }
            randomIndexesChosen[i] = randomIndex;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
