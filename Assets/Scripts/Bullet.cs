﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int bulletSpeed = 10;
    bool sleptEnemy = false;

    public GameObject zapEffect;
    ParticleSystem myZapEffect;

    void Start()
    {
        myZapEffect = GetComponentInChildren<ParticleSystem>();
    }

	// Use this for initialization
	void OnEnable () {
        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * bulletSpeed;
	}
	
	// Update is called once per frame
	void OnDisable () {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name=="NyanCat")
        {
            //Debug.Break();
            col.gameObject.GetComponent<AudioSource>().PlayOneShot(SoundBank.singleton.ricochet);
        }

        if (col.transform.tag == Tags.Manz && sleptEnemy == false)
        {
            Debug.Log("Hit enemy");
            gameObject.transform.parent = col.gameObject.transform;
            col.gameObject.GetComponent<TVManz>().sleep();
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            sleptEnemy = true;

            Instantiate(zapEffect, col.collider.gameObject.transform.position, transform.rotation);

            myZapEffect.Play();
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}