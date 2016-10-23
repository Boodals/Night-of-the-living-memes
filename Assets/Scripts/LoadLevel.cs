using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string start = "Start";
    public Fader m_fader;
    // Use this for initialization
    void Start ()
    {
        m_fader.setStartColour(Color.black);
        m_fader.fade(2.5f, Color.clear, 2.1f, 0.0f, null);
    }
	
	// Update is called once per frame
	void Update ()
    {
     
        if (Input.GetButtonDown(start))
        {
            m_fader.setStartColour(Color.clear);
            m_fader.fade(1.5f, Color.black, 0.0f, 0.0f, loadGame);
        }
    }

    private void loadGame()
    {
        SceneManager.LoadScene("asylumEditor");
    }
}
