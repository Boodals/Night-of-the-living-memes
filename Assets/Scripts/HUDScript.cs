using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public static HUDScript HUDsingleton;

    //crosshair stuff
    public Image crosshair_TL;
    public Image crosshair_TR;
    public Image crosshair_BL;
    public Image crosshair_BR;

    public float scale; //make private after debug

    //exit notif stuff
    public Image exitNotif;

    //ammo display
    public Text ammoTxt;
    public Image ammoImage;

    //timer display
    public Text time;
    public Image timerIcon;

    void Awake()
    {
        HUDsingleton = this;
    }

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //make the crosshairs move
        PositionCrosshairs(crosshair_TL, Vector3.up - Vector3.right);
        PositionCrosshairs(crosshair_TR, Vector3.up + Vector3.right);
        PositionCrosshairs(crosshair_BL, -Vector3.up - Vector3.right);
        PositionCrosshairs(crosshair_BR, -Vector3.up + Vector3.right);

        exitNotif.transform.localPosition += (Vector3.up * Mathf.Sin(Time.time * 1.3f)) * 0.1f;
	}

    public void SetCrosshairScale()
    {
        scale = 3.0f;
    }

    void PositionCrosshairs(Image crosshair, Vector3 direction)
    {
        Vector3 targetPosition = Vector3.zero + (direction * scale);
        crosshair.transform.localPosition = Vector3.Lerp(crosshair.transform.localPosition, targetPosition, Time.deltaTime * 3.0f);
    }

    public void ToggleExitNotif(bool exitAvailable)
    {
        if(exitAvailable)
        {
            exitNotif.enabled = true;
            
        }
        else
        {
            exitNotif.enabled = false;
        }
    }

    public void UpdateAmmoTotal(int curAmmoAmount)
    {
        ammoTxt.text = "" + curAmmoAmount + "/10";
    }

    public void UpdateTimer(int mins, int seconds)
    {
        if(mins == 0 && seconds > 9)
        {
            time.text = "0:" + seconds; 
        }
        if(mins == 0 && seconds < 10)
        {
            time.text = "0:0" + seconds; 
        }
        if(seconds > 9)
        {
            time.text = "" + mins + ":" + seconds;
        }
        if(seconds < 10)
        {
            time.text = "" + mins + ":0" + seconds; 
        }

    }
}
