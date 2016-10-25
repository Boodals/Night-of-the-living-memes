using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string start = "Start";
    public float m_fadeInDelay;
    public float m_fadeInDuration;
    public float m_fadeOutDelay;
    public float m_fadeOutDuration;
    public Fader m_fader;
    // Use this for initialization
    void Start ()
    {
        m_fader.setStartColour(Color.black);
        m_fader.fade(m_fadeInDuration, Color.clear, m_fadeInDelay, 0.0f, null);
    }
	
	// Update is called once per frame
	void Update ()
    {
     
        if (Input.GetButtonDown(start))
        {
            m_fader.setStartColour(Color.clear);
            m_fader.fade(m_fadeOutDuration, Color.black, 0.0f, m_fadeOutDelay, loadGame);
        }
    }

    private void loadGame()
    {
        SceneManager.LoadScene("asylumEditor");
    }
}
