using UnityEngine;
using System.Collections;

public class SoundBank : MonoBehaviour {
    public static SoundBank singleton;
    public AudioClip footstep1;

	// Use this for initialization
	void Awake () {
        singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
