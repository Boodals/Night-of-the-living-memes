using UnityEngine;
using System.Collections;

public class PhoneScript : MonoBehaviour {

    public bool onScreen = false;

    public Vector3 onScreenPos, offScreenPos;

	// Use this for initialization
	void Start () {
        onScreenPos = transform.localPosition;
        offScreenPos = transform.localPosition - Vector3.up * 1;
        offScreenPos -= Vector3.right * 1;
	}
	
	// Update is called once per frame
	void Update () {

        onScreen = (Input.GetAxisRaw("LT") > 0.6f);

        if(PlayerScript.playerSingleton.myState==PlayerScript.State.Dead)
        {
            onScreen = false;
        }




        Vector3 targetPos = offScreenPos;

        if(onScreen)
        {
            targetPos = onScreenPos;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 6 * Time.deltaTime);
	}


}
