using UnityEngine;
using System.Collections;

public class AmmoPickup : InteractiveMono {

    public int ammoAmount = 2;
    AudioSource snd;
    public AudioClip pickupSound;

	// Use this for initialization
	void Start () {
        m_canInteract = true;
        snd = GameObject.Find("Player").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void interact()
    {
        base.interact();
        snd.PlayOneShot(pickupSound, 0.75f);
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
