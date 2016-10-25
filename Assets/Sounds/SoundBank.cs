using UnityEngine;
using System.Collections;

public class SoundBank : MonoBehaviour {
    public static SoundBank singleton;
    public AudioClip[] playerFootsteps, gunShots;

    public AudioClip reloadSound, deathSound, outOfAmmoSound, ricochet;

	// Use this for initialization
	void Awake () {
        singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Pass an array of audio clips into this function to recieve a random choice back
    public AudioClip GetRandomClip(AudioClip[] sounds)
    {
        int index = Random.Range(0, sounds.Length);
        return sounds[index];
    }
}
