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

    public enum CrouchState { Standing, Crouching, Hiding}
    public CrouchState myCrouchState;

    public enum State { Standard, Reloading, Dead}
    public State myState;

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

        playerRigidbody.AddForce(oppositeForce * frictionValue);

        //Makes sure the player doesn't go above a certain speed

        if (currentSpeedMagnitude >= maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed;
        }

        //Fetches the input value input manager

        horizontalValue = Input.GetAxis(horizontal);
        verticalValue = Input.GetAxis(vertical);

        //Performs movement based on player input
        Vector3 movement = new Vector3(horizontalValue, 0, verticalValue);
        movement = transform.TransformDirection(movement);
        
        if(movement.magnitude>1)
        {
            movement = movement.normalized;
        }

        movement = LookForWalls(movement);

        playerRigidbody.AddForce(movement, ForceMode.VelocityChange);

        if(HUDScript.HUDsingleton)
            HUDScript.HUDsingleton.SetCrosshairScale();
	}

    Vector3 LookForWalls(Vector3 direction)
    {
        RaycastHit wall;
        float rayDistance = 0.65f;

        //This will be improved later probably
        if(Physics.SphereCast(transform.position, 0.6f, direction, out wall, rayDistance))
        {
            Vector3 temp = direction;
            //direction = Vector3.Cross(direction, wall.normal);
            //direction = Vector3.Cross(temp, direction);
            direction = Vector3.Lerp(direction, wall.normal, 0.5f);
        }

        return direction;
    }
}
