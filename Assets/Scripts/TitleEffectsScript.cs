using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleEffectsScript : MonoBehaviour {

    Shadow myShadow;
    Outline myOutline;

    float outlineSize = 3;

	// Use this for initialization
	void Start () {
        myShadow = GetComponent<Shadow>();
        myOutline = GetComponent<Outline>();
	}
	
	// Update is called once per frame
	void Update () {

        float curSize = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 3)) * outlineSize;

        Vector2 newSize = new Vector2(curSize, curSize);
        myOutline.effectDistance = newSize;


        float curZ = (Mathf.Sin(Time.timeSinceLevelLoad * 0.6f)) * 4;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, curZ);
    }
}
