using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{

    public Text[] m_names;
    public Text[] m_scores;
    public Text PlayerScore;

    public int m_index = -1;
	// Use this for initialization

	void Start () {
        HighScoreManager.Pair[] scores = HighScoreManager.g_instance.copyLeaderBoard();
        for (int i = 0; i < scores.Length; ++i)
        {
            if (i == m_index)
            {
                m_names[i].fontStyle = FontStyle.Bold;
                Outline outline = m_names[i].gameObject.AddComponent<Outline>();
                outline.effectColor = new Color(0,0.5f,0,0.7f);
                outline.effectDistance = new Vector2(10f, 10f);

                m_scores[i].fontStyle = FontStyle.Bold;
                outline = m_scores[i].gameObject.AddComponent<Outline>();
                outline.effectColor = new Color(0, 0.5f, 0, 0.7f);
                outline.effectDistance = new Vector2(10f,10f);
            }
            m_names[i].text = scores[i].name;
            m_scores[i].text = scores[i].score.ToString();
        }
        PlayerScore.text = PersistentScript.s_score.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
