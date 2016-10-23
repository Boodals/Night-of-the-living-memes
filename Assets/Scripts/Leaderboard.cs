using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{

    public Text[] m_names;
    public Text[] m_scores;
	// Use this for initialization

	void Start () {
        HighScoreManager.Pair[] scores = HighScoreManager.g_instance.copyLeaderBoard();
        for (int i = 0; i < scores.Length; ++i)
        {
            m_names[i].text = scores[i].name;
            m_scores[i].text = scores[i].score.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
