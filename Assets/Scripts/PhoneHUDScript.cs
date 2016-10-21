﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhoneHUDScript : MonoBehaviour
{
    //battery charge display stuff
    public Image[] batteryBars;
    public float lerpSpeed;
    int barNumber;

    //nyanometer display stuff
    public Image arrow;
    public float distance; //change this value when you want to change the rot of the nyanometer arrow
    public float farAway, nearby;

    public static PhoneHUDScript phoneHUDSingleton;

    void Awake()
    {
        phoneHUDSingleton = this;
    }
    

	// Use this for initialization
	void Start ()
    {
        barNumber = 3;
	}
	
	// Update is called once per frame
	void Update ()
    {
        batteryBars[barNumber].color = Color.Lerp(Color.clear, Color.white, Mathf.Abs(Mathf.Sin(Time.time * lerpSpeed)));

        float rotation = Mathf.Lerp(nearby, farAway, distance / 100);

        arrow.transform.localEulerAngles = new Vector3(0, 360, rotation);
	}

    void DisplayBatteryCharge()
    {
        if(FlashlightBehaviour.flashlightSingleton.batteryCharge > 75)
        {
            
            for(int i = 0; i < 4; i++)
            {
                batteryBars[i].enabled = true;
            }
            barNumber = 3;
        }
       if(FlashlightBehaviour.flashlightSingleton.batteryCharge <= 75)
        {
            Debug.Log("Battery below 75");
            batteryBars[3].enabled = false;
            barNumber = 2;
        }
       if(FlashlightBehaviour.flashlightSingleton.batteryCharge <= 50)
        {
            batteryBars[2].enabled = false;
            barNumber = 1;
        }
        if (FlashlightBehaviour.flashlightSingleton.batteryCharge <= 25)
        {
            batteryBars[1].enabled = false;
            barNumber = 0;
        }
        if(FlashlightBehaviour.flashlightSingleton.batteryCharge <= 0)
        {
            batteryBars[0].enabled = false;
            barNumber = 0;
        }
    }
}
