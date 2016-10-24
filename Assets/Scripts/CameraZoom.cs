using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

    float zoomSpeed = 0.3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.position.z < -12.25f)
        {

            gameObject.transform.position += Vector3.forward * zoomSpeed * Time.deltaTime;
        }
        else
        {
            zoomSpeed = 0.0f;
        }
	
	}
}
