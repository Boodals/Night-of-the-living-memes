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

    float scale; //make private after debug

    //exit notif stuff
    public Image exitNotif;

    //ammo display
    public Text ammoTxt;
    public Image ammoImage;

    //score display
    public Text scoreTxt;
    

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

    public void SetCrosshairScale(bool isCrouched, float scalingValue)
    {
        //call this to trigger the crosshairs' movement
        scale = -70 - (scalingValue * 10);

        if(isCrouched)
        {
            scale = scale * 0.7f;
        }
    }

    void PositionCrosshairs(Image crosshair, Vector3 direction)
    {
        //function to set the position of the crosshairs on the screen
        Vector3 targetPosition = Vector3.zero + (direction * scale);
        crosshair.transform.localPosition = Vector3.Lerp(crosshair.transform.localPosition, targetPosition, Time.deltaTime * 3.0f);
    }

    public void ToggleExitNotif(bool exitAvailable)
    {
        //turns on the exit icon to let player know they can leave the level
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

    public void UpdateScore(int score)
    {
        scoreTxt.text = "" + score;
    }
}
