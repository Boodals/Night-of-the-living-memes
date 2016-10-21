using UnityEngine;
using System.Collections;

public class FootstepBackup : MonoBehaviour {

    public AudioClip[] footsteps;
    public AudioSource m_oneShotSource;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Footstep()
    {
        //Debug.Break();
        m_oneShotSource.PlayOneShot(SoundBank.singleton.GetRandomClip(footsteps));
    }
}
