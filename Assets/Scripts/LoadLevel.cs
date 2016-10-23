using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string start = "Start";
    public Fader m_fader;
    // Use this for initialization
    void Start ()
    {
        m_fader.fadeIn(4.0f, Color.black, null, 0.5f);
    }
	
	// Update is called once per frame
	void Update ()
    {
     
        if (Input.GetButtonDown(start))
        {
            m_fader.fadeOut(2.0f, Color.black, loadGame);
        }
    }

    private void loadGame()
    {
        SceneManager.LoadScene("asylumEditor");
    }
}
