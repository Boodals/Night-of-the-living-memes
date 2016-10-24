using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhoneHUDScript : MonoBehaviour
{
    public Canvas phoneCanvas;

    //battery charge display stuff
    public Image[] batteryBars;
    public float lerpSpeed;
    int barNumber;

    //nyanometer display stuff
    public Image arrow;
    public float distance; //change this value when you want to change the rot of the nyanometer arrow
    public float farAway, nearby;

    //terminal count stuff
    public Text terminalCount;

    //timer display
    public Text time;

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
        if (FlashlightBehaviour.flashlightSingleton.myState == FlashlightBehaviour.flashlightState.On)
        {
            batteryBars[barNumber].color = Color.Lerp(Color.clear, Color.white, Mathf.Abs(Mathf.Sin(Time.time * lerpSpeed)));
        }
        DisplayBatteryCharge();

        float rotation = Mathf.Lerp(farAway, nearby, distance / 100);

        arrow.transform.localEulerAngles = new Vector3(0, 360, rotation);
	}

    void DisplayBatteryCharge()
    {
        if(FlashlightBehaviour.flashlightSingleton.batteryCharge > 75)
        {
            phoneCanvas.enabled = true;
            for (int i = 0; i < 4; i++)
            {
                batteryBars[i].enabled = true;
            }
            barNumber = 3;
        }
       if(FlashlightBehaviour.flashlightSingleton.batteryCharge <= 75)
        {
            phoneCanvas.enabled = true;
            Debug.Log("Battery below 75");
            batteryBars[3].enabled = false;
            barNumber = 2;
        }
       if(FlashlightBehaviour.flashlightSingleton.batteryCharge <= 50)
        {
            phoneCanvas.enabled = true;
            batteryBars[2].enabled = false;
            barNumber = 1;
        }
        if (FlashlightBehaviour.flashlightSingleton.batteryCharge <= 25)
        {
            phoneCanvas.enabled = true;
            batteryBars[1].enabled = false;
            barNumber = 0;
        }
        if(FlashlightBehaviour.flashlightSingleton.batteryCharge <= 0)
        {
            phoneCanvas.enabled = false;
            batteryBars[0].enabled = false;
            barNumber = 0;
        }
    }

    public void UpdateTerminalCount(int terminalsLeft)
    {
        terminalCount.text = "" + terminalsLeft + "/" + GameManager.gameManagerSingleton.m_exitManager.numTerminals();
    }

    public void UpdateTimer(int mins, int seconds)
    {
        if (mins == 0 && seconds > 9)
        {
            time.text = "0:" + seconds;
        }
        if (mins == 0 && seconds < 10)
        {
            time.text = "0:0" + seconds;
        }
        if (seconds > 9)
        {
            time.text = "" + mins + ":" + seconds;
        }
        if (seconds < 10)
        {
            time.text = "" + mins + ":0" + seconds;
        }
    }
}
