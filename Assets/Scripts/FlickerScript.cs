using UnityEngine;
using System.Collections;

public class FlickerScript : MonoBehaviour
{

    public Light myLight;

    enum states { normal, flickering, off }
    states currentState;

    float timer = 4, initialIntensity;

    float curTarget;
    float changeTargetTimer = 0.1f;

    // Use this for initialization
    void Start()
    {
        initialIntensity = myLight.intensity;

        SwitchState();
    }

    // Update is called once per frame
    void Update()
    {


        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SwitchState();
        }

        switch (currentState)
        {
            case states.flickering:
                if (changeTargetTimer <= 0)
                {
                    changeTargetTimer = 0.1f;
                    curTarget = Random.Range(0, initialIntensity * 1.2f);
                }
                else
                {
                    changeTargetTimer -= Time.deltaTime;
                }
                break;
            case states.normal:
                curTarget = initialIntensity;
                break;
            case states.off:
                curTarget = 0;
                break;
        }

        myLight.intensity = Mathf.Lerp(myLight.intensity, curTarget, 3 * Time.deltaTime);
    }

    void SwitchState()
    {
        timer = Random.Range(3, 6);

        currentState = (states)Random.Range(0, 2);

        if (currentState == states.flickering)
            timer *= 0.2f;
    }
}
