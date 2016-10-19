using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript playerSingleton;

    //public String jumpButton = "Jump"; //Input Manager name here
    public string horizontal = "Horizontal";
    public string vertical = "Vertical";
    public string crouch = "Crouch";

    public float playerSpeed;
    public float maxSpeed;
    public float frictionValue;

    float currentSpeedMagnitude; //Current speed 'Magnitude of the velocity of the rigidbody'
    float horizontalValue;
    float verticalValue;
    float crouchValue;

    Vector3 currentVelocity;
    Vector3 oppositeForce;
    Rigidbody playerRigidbody;

    public enum CrouchState { Standing, Crouching, Hiding}
    public CrouchState myCrouchState;
    public Vector3[] cameraPositions;

    public enum State { Standard, Reloading, Dead}
    public State myState;

    public Vector3 currentLookDirection;
    public Camera myCamera;

    public static float sensitivity = 170;

    float currentNoise = 0;
    float movementIntensity = 0;

    void Awake()
    {
        playerSingleton = this;

    }

	// Use this for initialization
	void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
        currentLookDirection = transform.forward;

        myCamera = gameObject.GetComponentInChildren<Camera>();
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
        crouchValue = Input.GetAxis(crouch);

        //Performs movement based on player input
        Vector3 movement = new Vector3(horizontalValue, 0, verticalValue);
        movement = myCamera.transform.TransformDirection(movement);
        movement.y = 0;

        if (movement.magnitude > 1)
        {
            movementIntensity = movement.magnitude;
            movement = movement.normalized;
        }

        movement = LookForWalls(movement);

        playerRigidbody.AddForce(movement, ForceMode.VelocityChange);

        HandleCrouching();
        HandleNoise();
        LookingAround();

        if (HUDScript.HUDsingleton)
            HUDScript.HUDsingleton.SetCrosshairScale();
    }

    void HandleCrouching()
    {
        Vector3 targetCamPos = cameraPositions[(int)myCrouchState];

        if (crouchValue == 1)
        {
            myCrouchState = CrouchState.Crouching;
        }
        else
        {
            myCrouchState = CrouchState.Standing;
        }

        myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, targetCamPos, 4 * Time.deltaTime);
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

    void HandleNoise()
    {
        currentNoise = Mathf.Lerp(currentNoise, -1, 5 * Time.deltaTime);
    }

    public void MakeNoise(float amount = 1)
    {
        currentNoise += amount;
        Debug.Log("Current noise level is " + currentNoise);
    }

    void Footstep()
    {
        MakeNoise(1);
    }

    void LookingAround()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Vertical2"), Input.GetAxisRaw("Horizontal2"), 0) * sensitivity;

        currentLookDirection += input * Time.deltaTime;

        currentLookDirection.x = Mathf.Clamp(currentLookDirection.x, -65, 50);
        //currentLookDirection = currentLookDirection.normalized;

        myCamera.transform.eulerAngles = currentLookDirection;
        //Debug.DrawLine(transform.position, transform.position + currentLookDirection, Color.red, 10);
        //myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, Quaternion.LookRotation(currentLookDirection), 10 * Time.deltaTime);
    }

    public float GetCurrentNoiseLevel()
    {
        return currentNoise;
    }
}
