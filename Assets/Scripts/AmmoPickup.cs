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

    public override void interact()
    {
        if (GunScript.ammo != 10 && GunScript.ammo != 9)
        {
            base.interact();
            snd.PlayOneShot(pickupSound, 0.75f);
            GunScript.gunSingleton.ChangeAmmo(ammoAmount);
            Destroy(gameObject);
        }
        else if (GunScript.ammo == 9)
        {
            base.interact();
            snd.PlayOneShot(pickupSound, 0.75f);
            GunScript.gunSingleton.ChangeAmmo(1);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Ammo full!");
        }
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
