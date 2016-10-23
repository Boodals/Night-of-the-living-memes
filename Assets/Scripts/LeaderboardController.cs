using UnityEngine;
using System.Collections;

public class LeaderboardController : MonoBehaviour {

    public GameObject nameInput;
    public GameObject leaderboard;
    public Fader m_fader;
	// Use this for initialization
	void Start () {
        nameInput.SetActive( false);
        leaderboard.SetActive(false);

        if  (HighScoreManager.g_instance.scoreIsHighScore(PersistentScript.s_score))
        {
            nameInput.SetActive(true);
        }
        else
        {
            leaderboard.SetActive(true);
        }

        m_fader.fadeIn(4.0f,Color.black, null,0.5f);
    }
    public void showLeaderboard()
    {
        leaderboard.SetActive(true);
    }	
	// Update is called once per frame
	void Update () {
	
	}
}
