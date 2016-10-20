using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour {

    public int ammoAmount = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GunScript.gunSingleton.ChangeAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
