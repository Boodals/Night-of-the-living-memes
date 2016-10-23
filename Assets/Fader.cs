using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    public float m_fadeDuration;
    public Image m_fader;
    public float m_fadeLinger;

    public Color m_fadeTarget;
    private Color m_startCol;
    private float m_timer;
    private FADE m_fadeState;
    private enum FADE
    {
        OUT,
        IN,
        NONE
    }
    // Use this for initialization
    void Start ()
    {
        m_timer = 0.0f;
        m_fadeState = FADE.IN;
    }

    private void fadeIn()
    {
        m_fadeState = FADE.IN;
    }

    public void fadeOut()
    {
        m_fadeState = FADE.OUT;
    }
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime;
        switch (m_fadeState)
        {
            case FADE.OUT:
                m_fader.color = Color.Lerp(m_startCol, m_fadeTarget, m_timer / m_fadeDuration);
                if (m_timer >= m_fadeDuration + m_fadeLinger)
                {
                    //done
                    m_fadeState = FADE.NONE;
                }
                break;
            case FADE.IN:
                m_fader.color = Color.Lerp(m_fadeTarget, m_startCol, m_timer / m_fadeDuration);
                if (m_timer >= m_fadeDuration)
                {
                    m_fadeState = FADE.NONE;
                    m_timer = 0.0f;
                }
                break;
            case FADE.NONE:
                m_timer = 0.0f;
                break;
            default:
                break;
        }
    }
}
