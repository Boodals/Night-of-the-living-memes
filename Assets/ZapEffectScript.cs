using UnityEngine;
using System.Collections;

public class ZapEffectScript : MonoBehaviour {

    Light myLight;

    float curIntensity = 8;
    Vector3 pos;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 10);
        myLight = GetComponent<Light>();
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        curIntensity -= Time.deltaTime * 3;

        float flickerIntensity = Random.Range(curIntensity / 2, curIntensity * 2);

        myLight.intensity = Mathf.Lerp(myLight.intensity, flickerIntensity, 28 * Time.deltaTime);

        Vector3 randomPos = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        randomPos *= 0.5f;
        randomPos += pos;

        transform.position = Vector3.Lerp(transform.position, randomPos, 10 * Time.deltaTime);
	}
}
