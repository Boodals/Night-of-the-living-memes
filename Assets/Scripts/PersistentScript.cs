using UnityEngine;
using System.Collections;

public class PersistentScript : MonoBehaviour
{

    public static int s_score;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}

}
