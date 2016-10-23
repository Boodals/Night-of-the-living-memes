using UnityEngine;
using System.Collections;

public class LeaderboardController : MonoBehaviour {

    public Canvas nameInput;
    public Canvas leaderboard;

	// Use this for initialization
	void Start () {
        nameInput.enabled = false;
        leaderboard.enabled = false;
        if  (HighScoreManager.g_instance.scoreIsHighScore(PersistentScript.s_score))
        {
            nameInput.enabled = true;
        }
        else
        {
            leaderboard.enabled = true;
        }
	}
    public void showLeaderboard()
    {
        leaderboard.enabled = true;
    }	
	// Update is called once per frame
	void Update () {
	
	}
}
