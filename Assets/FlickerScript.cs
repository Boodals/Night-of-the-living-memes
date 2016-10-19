using UnityEngine;
using System.Collections;

public class FlickerScript : MonoBehaviour
{

    public Light myLight;

    enum states { normal, flickering, off }
    states currentState;

    float timer = 4, initialIntensity;

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
                myLight.intensity = Random.Range(0, initialIntensity*1.2f);
                break;
            case states.normal:
                myLight.intensity = initialIntensity;
                break;
            case states.off:
                myLight.intensity -= Time.deltaTime * 2;
                break;
        }
    }

    void SwitchState()
    {
        timer = Random.Range(3, 6);

        currentState = (states)Random.Range(0, 2);

        if (currentState == states.flickering)
            timer *= 0.2f;
    }
}
