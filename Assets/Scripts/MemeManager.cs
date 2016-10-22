using UnityEngine;
using System.Collections;

public class MemeManager : MonoBehaviour {

    public GameObject plane;
    Texture memeImage;
    public Texture[] memeImages;
    int randomArrayIndex;
    int memeAmount = 20;

    void Start()
    {
        randomArrayIndex = Random.Range(0, memeAmount);
        memeImages = Resources.LoadAll<Texture>("Memes");
        memeImage = memeImages[randomArrayIndex];

        Material matToChange = plane.GetComponent<Renderer>().material;

        //matToChange.SetTexture("_MainTexture", memeImage);
        matToChange.SetTexture("_EmissionMap", memeImage);
    }

    void Update()
    {

    }
	
}