using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int bulletSpeed = 10;

	// Use this for initialization
	void OnEnable () {
        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * bulletSpeed;
        gameObject.transform.Rotate(Vector3.right * 90);
	}
	
	// Update is called once per frame
	void OnDisable () {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col);
        if (col.transform.tag == Tags.Manz)
        {
            col.gameObject.GetComponent<TVManz>().sleep();
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}