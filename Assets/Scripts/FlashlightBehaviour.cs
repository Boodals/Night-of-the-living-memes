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
    PhoneScript playerPhone;

    public Light flashlight;
    public static FlashlightBehaviour flashlightSingleton;

    void Awake()
    {
        playerPhone = GameObject.Find("Phone").GetComponent<PhoneScript>();
        flashlightSingleton = this;
    }

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
        if (playerPhone.onScreen == true)
        {
            toggledThisFrame = true;
            myState = flashlightState.On;
            flashlight.enabled = true;
        }
        else
        {
            myState = flashlightState.Off;
            flashlight.enabled = false;
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
