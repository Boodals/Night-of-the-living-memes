using UnityEngine;
using System.Collections;

public class MemeManager : MonoBehaviour {

    public GameObject plane;
    Material memeImage;
    int randomArrayIndex;
    int memeAmount = 19;

    void Start()
    {
        randomArrayIndex = Random.Range(0, memeAmount);
        Material[] memeImages = Resources.LoadAll<Material>("Materials");
        memeImage = memeImages[randomArrayIndex];
        plane.GetComponent<Renderer>().material = memeImage;
    }

    void Update()
    {

    }
	
}