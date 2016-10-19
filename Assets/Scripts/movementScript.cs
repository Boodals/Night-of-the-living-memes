using UnityEngine;
using System.Collections;

public class movementScript : MonoBehaviour {

    //public String jumpButton = "Jump"; //Input Manager name here
    //public String crouchButton = "Crouch"; //Input Manager name here
    public string horizontal = "Horizontal";
    public string vertical = "Vertical";
    public float playerSpeed;
    public float maxSpeed;
    public float frictionValue;
    public float currentSpeedMagnitude; //Current speed 'Magnitude of the velocity of the rigidbody'
    public float horizontalValue;
    public float verticalValue;
    public Vector3 currentVelocity;
    public Vector3 oppositeForce;
    public Rigidbody playerRigidbody;

	// Use this for initialization
	void Start () {
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //Fetches rigidbody velocity values

        currentSpeedMagnitude = playerRigidbody.velocity.magnitude;
        currentVelocity = playerRigidbody.velocity;
        oppositeForce = -currentVelocity;

        //Applies friction

        playerRigidbody.AddRelativeForce(oppositeForce * frictionValue);

        //Makes sure the player doesn't go above a certain speed

        if (currentSpeedMagnitude >= maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed;
        }

        //Fetches the input value input manager

        horizontalValue = Input.GetAxis(horizontal);
        verticalValue = Input.GetAxis(vertical);

        //Performs movement based on player input

        if (verticalValue > 0)
        {
            playerRigidbody.AddForce(Vector3.forward * playerSpeed);
        }
        else if (verticalValue < 0)
        {
            playerRigidbody.AddForce(Vector3.back * playerSpeed);
        }
        else if (horizontalValue > 0)
        {
            playerRigidbody.AddForce(Vector3.right * playerSpeed);
        }
        else if (horizontalValue < 0)
        {
            playerRigidbody.AddForce(Vector3.left * playerSpeed);
        }
	
	}
}
