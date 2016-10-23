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

    //nyan cat notifcation stuff
    public Text nyanNotif;
    float nyanTimer = 6.0f;
    float fadeTime = 4.0f;
    public bool displayNotif;
    

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

        if(displayNotif)
        {
            nyanTimer -= Time.deltaTime * 0.5f;

            if (nyanTimer <= fadeTime)
            {
                nyanNotif.color = Color.Lerp(nyanNotif.color, Color.clear, Time.deltaTime * 0.4f);
            }

            if(nyanTimer <= 0)
            {
                displayNotif = false;
            }
        }
        else
        {
            nyanNotif.enabled = false;
        }

	}

    public void SetCrosshairScale(bool isCrouched, float scalingValue)
    {
        //call this to trigger the crosshairs' movement
        scale = -20 - (scalingValue * 30);

        if(isCrouched)
        {
            scale = scale * 0.4f;
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

    public void DisplayNyanNotif()
    {
        nyanTimer = 6.0f;    
        displayNotif = true;
        nyanNotif.enabled = true;
        nyanNotif.color = Color.white;
    }
}
