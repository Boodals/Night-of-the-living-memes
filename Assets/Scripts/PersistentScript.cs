using UnityEngine;
using System.Collections;

public class PersistentScript : MonoBehaviour
{

    public static int s_score;
    private static PersistentScript m_inst;

	// Use this for initialization
	void Start ()
    {
        if (m_inst == null)
        {
            m_inst = this;
            DontDestroyOnLoad(gameObject);
        }
	}

}
