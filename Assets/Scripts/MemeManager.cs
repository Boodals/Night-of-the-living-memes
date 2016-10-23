using UnityEngine;
using System.Collections;

public class MemeManager : MonoBehaviour {

    public bool dynamicMemeGeneration;
    public int randomDuration;
    public float timeTaken;
    Texture randomImage;
    public GameObject plane;
    Texture memeImage;
    public Texture[] memeImages;
    int randomArrayIndex;
    int memeAmount = 20;
    Material matToChange;

    void Start()
    {
        randomArrayIndex = Random.Range(0, memeAmount);
        memeImages = Resources.LoadAll<Texture>("Memes");
        memeImage = memeImages[randomArrayIndex];

        matToChange = plane.GetComponent<Renderer>().material;

        //matToChange.SetTexture("_MainTexture", memeImage);
        matToChange.SetTexture("_MainTex", memeImage);

        randomDuration = Random.Range(2, 8);
    }

    void Update()
    {
        if (dynamicMemeGeneration == true)
        {
            timeTaken += Time.deltaTime;
            if (timeTaken >= randomDuration)
            {
                randomDuration = Random.Range(2, 8);
                randomArrayIndex = Random.Range(0, memeAmount);
                randomImage = memeImages[randomArrayIndex];
                matToChange.SetTexture("_EmissionMap", randomImage);
                timeTaken = 0.0f;
            }
           
            
        }

    }
	
}