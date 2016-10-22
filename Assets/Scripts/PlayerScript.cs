﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript playerSingleton;

    FlashlightBehaviour playerFlashlight;

    //public String jumpButton = "Jump"; //Input Manager name here
    public string horizontal = "Horizontal";
    public string vertical = "Vertical";
    public string crouch = "Crouch";
    public string sprint = "Sprint";

    public float playerSpeed;
    public float maxSpeed;
    public float frictionValue;

    float currentSpeedMagnitude; //Current speed 'Magnitude of the velocity of the rigidbody'
    float horizontalValue;
    float verticalValue;
    float crouchValue;
    float sprintValue;

    Vector3 currentVelocity;
    Vector3 oppositeForce;
    Rigidbody playerRigidbody;

    public enum CrouchState { Standing, Crouching, Hiding}
    public CrouchState myCrouchState;
    Vector3[] cameraPositions;

    public enum State { Standard, Sprinting, Reloading, Dead}
    public State myState;

    public Vector3 currentLookDirection;
    public Camera myCamera;

    public static float sensitivity = 170;

    float currentNoise = 0;
    public float movementIntensity = 0;

    float curBobAmount = 0;

    AudioSource snd;
    bool waitingForFootstep = true;
    public bool flashlightOn;
    public bool flashlightToggledThisFrame;

    void Awake()
    {
        playerSingleton = this;

        snd = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
        currentLookDirection = transform.forward;

        playerFlashlight = gameObject.GetComponentInChildren<FlashlightBehaviour>();
        myCamera = gameObject.GetComponentInChildren<Camera>();

        cameraPositions = new Vector3[3];
        cameraPositions[0] = new Vector3(0, 1.1f, 0);
        cameraPositions[1] = new Vector3(0, 0.3f, 0.0f);
        cameraPositions[2] = new Vector3(0, -0.65f, 0.35f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        flashlightOn = playerFlashlight.isFlashlightOn();
        flashlightToggledThisFrame = playerFlashlight.toggledThisFrame;
        float currentMaxSpeedMultiplier = 1 / ((int)myCrouchState+1);

        if (myState == State.Sprinting)
            currentMaxSpeedMultiplier *= 2;

        //Fetches rigidbody velocity values
        currentSpeedMagnitude = playerRigidbody.velocity.magnitude;
        currentVelocity = playerRigidbody.velocity;
        oppositeForce = -currentVelocity;

        //Applies friction
        playerRigidbody.AddForce(oppositeForce * frictionValue);

        //Makes sure the player doesn't go above a certain speed
        if (currentSpeedMagnitude >= maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed * currentMaxSpeedMultiplier;
        }

        //Fetches the input value input manager
        horizontalValue = Input.GetAxis(horizontal);
        verticalValue = Input.GetAxis(vertical);
        sprintValue = Input.GetAxis(sprint);

        //Performs movement based on player input
        Vector3 movement = new Vector3(horizontalValue, 0, verticalValue);
        movement = myCamera.transform.TransformDirection(movement);
        movement.y = 0;

        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        movement = LookForWalls(movement);

        playerRigidbody.AddForce(movement, ForceMode.VelocityChange);
        movementIntensity = movement.magnitude;

        HandleHeadbob();
        HandleCrouching();
        HandleSprinting();
        HandleNoise();
        LookingAround();

        if (HUDScript.HUDsingleton)
        {
            HUDScript.HUDsingleton.SetCrosshairScale(myCrouchState == CrouchState.Crouching, movementIntensity);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            MakeNoise(100);
        }
    }

    void HandleHeadbob()
    {
        float targetBobAmount = movementIntensity * 0.29f;
        float bobSpeed = 1 + (5 * movementIntensity);

        if (myState == State.Sprinting)
        {
            targetBobAmount = 1.55f;
            bobSpeed = 12;
        }

        float sinAmount = Mathf.Sin(Time.timeSinceLevelLoad * bobSpeed);

        if (movementIntensity > 0.1f)
        {
            if (Mathf.Abs(sinAmount) > 0.5f && waitingForFootstep)
            {
                waitingForFootstep = false;
                Footstep();
            }

            if (Mathf.Abs(sinAmount) < 0.1f)
            {
                waitingForFootstep = true;
            }
        }
        

        curBobAmount = Mathf.Lerp(curBobAmount, sinAmount * targetBobAmount, 4 * Time.deltaTime);

        //Final position is applied in the crouch bit
    }

    void HandleSprinting()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            if (myState == State.Standard && myCrouchState != CrouchState.Crouching)
            {
                myState = State.Sprinting;
                //playerSpeed = 655;
                //maxSpeed = 6;
            }
            else
            {
                myState = State.Standard;
                //playerSpeed = 455;
                //maxSpeed = 3;
            }
        }

        if((myState==State.Sprinting && movementIntensity<0.9f) || myCrouchState==CrouchState.Crouching)
        {
            myState = State.Standard;
        }
    }

    public void AlignWithSpawnPoint(Transform spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
        currentLookDirection = spawnPoint.transform.eulerAngles;
    }

    void HandleCrouching()
    {
        Vector3 targetCamPos = myCamera.transform.InverseTransformDirection(cameraPositions[(int)myCrouchState]);

        if (Input.GetButtonDown("Crouch"))
        {
            if(myCrouchState == CrouchState.Standing)
            {
                myCrouchState = CrouchState.Crouching;
                playerSpeed = 255;
                maxSpeed = 1.5f;
            }
            else
            {
                myCrouchState = CrouchState.Standing;
                playerSpeed = 455;
                maxSpeed = 3;
            }
        }

        targetCamPos -= Vector3.up * curBobAmount;

        myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, targetCamPos, 4 * Time.deltaTime);
    }

    Vector3 LookForWalls(Vector3 direction)
    {
        RaycastHit wall;
        float rayDistance = 0.65f;

        //This will be improved later probably
        if(Physics.SphereCast(transform.position, 0.6f, direction, out wall, rayDistance, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            Vector3 temp = Vector3.Cross(wall.normal, direction);
            //direction = Vector3.Cross(temp, wall.normal);
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
        //Debug.Log("Current noise level is " + currentNoise);
    }

    void Footstep()
    {
        snd.PlayOneShot(SoundBank.singleton.GetRandomClip(SoundBank.singleton.playerFootsteps), movementIntensity * 0.5f);

        float footstepNoise = movementIntensity;

        if(myCrouchState==CrouchState.Crouching)
        {
            footstepNoise *= 0.1f;
        }

        MakeNoise(footstepNoise);
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

    public Camera GetCamera()
    {
        return myCamera;
    }
}
