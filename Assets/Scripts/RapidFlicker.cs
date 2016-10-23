using UnityEngine;
using System.Collections;

public class RapidFlicker : MonoBehaviour {

    Light objectLight;

    float timeOn;
    float timeOff;

    float changeTime = 0;

	// Use this for initialization
	void Start () {
        objectLight = gameObject.GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        timeOn = Random.Range(0.1f, 0.5f);
        timeOff = Random.Range(0.6f, 1.0f);
        if (Time.time > changeTime)
        {
            objectLight.enabled = !objectLight.enabled;
            if (objectLight.enabled)
            {
                changeTime = Time.time + timeOn;
            }
            else
            {
                changeTime = Time.time + timeOff;
            }
        }
    }
}
