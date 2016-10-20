using UnityEngine;
using System.Collections;

public class MemeManager : MonoBehaviour {

    public GameObject plane;
    Texture memeImage;
    public Texture[] memeImages;
    int randomArrayIndex;
    int memeAmount = 19;

    void Start()
    {
        randomArrayIndex = Random.Range(0, memeAmount);
        memeImages = Resources.LoadAll<Texture>("Memes");
        memeImage = memeImages[randomArrayIndex];

        Material matToChange = plane.GetComponent<Renderer>().material;

        //matToChange.SetTexture("_MainTexture", memeImage);
        matToChange.mainTexture = memeImage;
    }

    void Update()
    {

    }
	
}