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
        Debug.Log(PersistentScript.s_score);
        if  (HighScoreManager.g_instance.scoreIsHighScore(PersistentScript.s_score))
        {
            nameInput.SetActive(true);
        }
        else
        {
            leaderboard.SetActive(true);
        }

    }
    public void showLeaderboard()
    {
        leaderboard.SetActive(true);
    }	
	// Update is called once per frame
	void Update () {
	
	}
}
