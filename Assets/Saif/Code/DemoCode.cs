using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoCode : MonoBehaviour
{

    public InputField _userName;
    public InputField R_userID;
    public InputField Leaderboard_userID_Save;
    public InputField newLeaderboardScore;
    public InputField DailyLeaderboardScore_userID_Save;
    public InputField newDailyLeaderboardScore;
    public InputField LeaderboardScore_userID_load;
    public Text fetchedLeaderboardScore;
    public InputField DailyLeaderboardScore_userID_load;
    public Text fetchedDailyLeaderboardScore;

    public void RegisterUser()
    {
        UsersManager._instance.registerUser(_userName.text, R_userID.text);
    }
    public void setLeaderboardScore()
    {
        DatabaseManager._instance.setScoreInLeaderboard(Leaderboard_userID_Save.text, int.Parse(newLeaderboardScore.text));
    }
    public void setDailyLeaderboardScore()
    {
        DatabaseManager._instance.setScoreInDailyLeaderboard(DailyLeaderboardScore_userID_Save.text, int.Parse(newDailyLeaderboardScore.text));
    }
    public void loadLeaderboardScore()
    {
        Debug.Log("FETCHED SCORE");
        DatabaseManager._instance.getLeaderboardScore(LeaderboardScore_userID_load.text, loadedScore =>
         {
             fetchedLeaderboardScore.text = "SCORE IS : " + loadedScore;
         });
    }
    public void loadDailyLeaderboardScore()
    {
        Debug.Log("FETCHED DAILY SCORE");
        DatabaseManager._instance.getDailyLeaderboardScore(LeaderboardScore_userID_load.text, loadedScore =>
         {
             fetchedDailyLeaderboardScore.text = "SCORE IS : " + loadedScore;
         });
    }
}
