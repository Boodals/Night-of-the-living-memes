using UnityEngine;
using System.Collections;

public class AmmoPickup : Interactable {

    public int ammoAmount = 2;

	// Use this for initialization
	void Start () {
        m_canInteract = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void interact()
    {
        base.interact();

        GunScript.gunSingleton.ChangeAmmo(ammoAmount);
        Destroy(gameObject);
    }

    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.tag == "Player")
    //    {
    //        GunScript.gunSingleton.ChangeAmmo(ammoAmount);
    //        Destroy(gameObject);
    //    }
    //}
}
