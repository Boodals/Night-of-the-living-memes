using UnityEngine;
using System.Collections;

public class FootstepBackup : MonoBehaviour {

    public AudioClip[] footsteps;
    public AudioSource m_oneShotSource;

    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Footstep()
    {
        //Debug.Break();
        m_oneShotSource.PlayOneShot(SoundBank.singleton.GetRandomClip(footsteps), anim.GetFloat("MovementIntensity")/3);
    }
}
