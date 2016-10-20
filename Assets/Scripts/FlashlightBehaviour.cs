using UnityEngine;
using System.Collections;

public class FlashlightBehaviour : MonoBehaviour {

    public string flashlightButton = "Flashlight";

    public float batteryCharge = 100;
    public float batteryDeclineRate = 0.5f;
    public bool toggledThisFrame;
    //Used enum incase we want to add more functionality for the flashlight later on
    public enum flashlightState {On, Off}
    public flashlightState myState;

    public Light flashlight;


	// Use this for initialization
	void Start () {
        flashlight = gameObject.GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        if (myState == flashlightState.On)
        {
            batteryCharge -= Time.deltaTime * batteryDeclineRate;
        }
        if (Input.GetButtonDown(flashlightButton))
        {
            toggledThisFrame = true;
            if (myState == flashlightState.On)
            {
                myState = flashlightState.Off;
                flashlight.enabled = false;
            }
            else if (myState == flashlightState.Off)
            {
                myState = flashlightState.On;
                flashlight.enabled = true;
            }
        }
        else
        {
            toggledThisFrame = false;
        }


	}

    public bool isFlashlightOn()
    {
        if (myState == flashlightState.On)
        {
            return true;
        }
        else
        {
            return false;
        }
               
    }
}
