using UnityEngine;
using System.Collections;

public class ScreenEffectScript : MonoBehaviour {

    float changeTimer = 0;

    float waitTimer = 0;

    Material myMat;

    float eA = 0;

	// Use this for initialization
	void Start () {
        myMat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

        if(changeTimer<=0)
        {
            eA = Mathf.Lerp(eA, 0, 18 * Time.deltaTime);

            if (waitTimer <= 0)
            {
                if (Random.Range(0, 7) > 2)
                {
                    changeTimer = Random.Range(0.1f, 0.7f);
                    eA = Random.Range(-0.4f, 1.9f);
                    eA += changeTimer;

                    waitTimer = changeTimer * 2;
                }
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }
        else
        {
            changeTimer -= Time.deltaTime;

            
        }


        myMat.SetFloat("_EffectAmount", eA);
	}
}
