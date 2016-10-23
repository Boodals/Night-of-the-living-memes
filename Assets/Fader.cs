using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    public delegate void FadeCallback();

    public Image m_fader;
    private float m_fadeDuration;
    
    private float m_startWait;
    private float m_endWait;

    private Color m_from;
    private Color m_to;
    private float m_timer;
    private float m_startTimer;
    private float m_endTimer;
    FadeCallback m_callback;
    private bool m_fading;
    // Use this for initialization
    void Start()
    {
    }
    public void setStartColour(Color _startCol, bool _override = false)
    {
        if (m_fading && !_override)
        {
            m_from = m_fader.color;
        }
        else
        {
            m_from = _startCol;
        }

        m_fader.color = m_from;
    }
    public void fade(float _duration, Color _to, float _startWait = 0.0f, float _endWait = 0.0f, FadeCallback _callback = null)
    {
        m_fadeDuration = _duration;
        m_to = _to;
        m_startWait = _startWait;
        m_endWait = _endWait;
        m_callback = _callback;

        m_startTimer = 0;
        m_endTimer = 0;
        m_timer = 0;

        m_fading = true;
    }

    void finished()
    {
        if (m_callback != null) m_callback();
        m_fadeDuration = 0.0f;
        m_startTimer = 0.0f;
        m_timer = 0.0f;
        m_endTimer = 0.0f;

        m_fading = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_fading)
        {
            if (m_startTimer <= m_startWait)
            {
                //Debug.Log("Started fade");
                m_startTimer += Time.deltaTime;
            }
            else if (m_timer <= m_fadeDuration)
            {
                //fade
                m_timer += Time.deltaTime;
                m_fader.color = Color.Lerp(m_from, m_to, m_timer / m_fadeDuration);
            }
            else if (m_endTimer <= m_endWait)
            {
                m_endTimer += Time.deltaTime;
            }
            else
            {
                finished();
            }

        }
    }
}

//   public delegate void FadeCallback();

//   public Image m_fader;
//   private float m_fadeDuration;
//   private float m_fadeLinger;

//   private Color m_fadeTarget;
//   private Color m_startCol;
//   private float m_timer;
//   private FADE m_fadeState;
//   FadeCallback m_callback;
//   float m_lingerTimer;
//   private enum FADE
//   {
//       OUT,
//       IN,
//       SOLID,
//       NONE
//   }
//   // Use this for initialization
//   void Start ()
//   {
//       m_timer = 0.0f;
//       m_fadeState = FADE.NONE;
//       m_startCol = Color.clear;

//   }

//   public void fadeIn(float _duration, Color _col, FadeCallback _callback=null, float _linger = 0.0f)
//   {
//       m_fadeLinger = _linger;
//       m_fadeDuration = _duration;
//       m_fadeTarget = _col;
//       m_callback = _callback;
//       m_fadeState = FADE.SOLID;
//       m_timer = 0.0f;
//       m_lingerTimer = 0.0f;
//       m_fader.color = m_fadeTarget;
//       m_startCol = Color.clear;
//   }

//   public void fadeOut(float _duration, Color _col, FadeCallback _callback = null, float _linger =0.0f)
//   {
//       m_fadeLinger = _linger;
//       m_fadeDuration = _duration;
//       m_fadeTarget = _col;
//       m_callback = _callback;
//       m_fadeState = FADE.OUT;
//       m_timer = 0.0f;
//       m_lingerTimer = 0.0f;
//       if (m_fadeState != FADE.NONE)
//       {
//           m_startCol = m_fader.color;
//       }
//       else
//       {
//           m_startCol = Color.clear;
//       }
//   }
//// Update is called once per frame
//void Update ()
//   {
//       m_timer += Time.deltaTime;
//       m_lingerTimer += Time.deltaTime;
//       switch (m_fadeState)
//       {
//           case FADE.OUT:
//               m_fader.color = Color.Lerp(m_startCol, m_fadeTarget, m_timer / m_fadeDuration);
//               if (m_timer >= m_fadeDuration + m_fadeLinger)
//               {
//                   m_fadeState = FADE.NONE;
//                   if (m_callback !=null) m_callback();
//               }
//               break;
//           case FADE.IN:
//               m_fader.color = Color.Lerp(m_fadeTarget, m_startCol, m_timer / m_fadeDuration);
//               if (m_timer >= m_fadeDuration)
//               {
//                   m_fadeState = FADE.NONE;
//                   if (m_callback != null) m_callback();
//               }

//               break;  
//           case FADE.SOLID:
//               if (m_timer >= m_fadeLinger)
//               {
//                   m_fadeState = FADE.IN;
//                   m_timer = 0.0f;
//               }
//               break;
//           case FADE.NONE:
//               m_timer = 0.0f;
//               break;
//           default:
//               break;
//       }
//   }