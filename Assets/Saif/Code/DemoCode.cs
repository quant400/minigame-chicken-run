using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoCode : MonoBehaviour
{
    public InputField _userName;
    public InputField assetID;
    public TextMeshProUGUI LeaderboardScore;
    public TextMeshProUGUI DailyLeaderboardScore;
    public TextMeshProUGUI SessionsCounter;
    

    public void RegisterUser()
    {
        UsersManager._instance.registerUser(_userName.text,assetID.text);
    }

    private void Start() {
        InvokeRepeating("loadLeaderboardScore",1f,2.5f);
        InvokeRepeating("loadSessionCounter",1f,2.5f);
        InvokeRepeating("loadDailyLeaderboardScore",1f,2.5f);
    }
    // public void setLeaderboardScore()
    // {
    //     DatabaseManager._instance.setScoreInLeaderboard(Leaderboard_userID_Save.text,int.Parse(newLeaderboardScore.text));
    // }
    // public void setDailyLeaderboardScore()
    // {
    //     DatabaseManager._instance.setScoreInDailyLeaderboard(DailyLeaderboardScore_userID_Save.text,int.Parse(newDailyLeaderboardScore.text));
    // }
    public void loadLeaderboardScore()
    {
        LeaderboardScore.text = "Leaderboard Score: "+DatabaseManager._instance.getLS();
    }
    public void loadSessionCounter()
    {
        SessionsCounter.text = "Sessions today: "+DatabaseManager._instance.getSessionsCounter();
    }
    public void loadDailyLeaderboardScore()
    {
        DailyLeaderboardScore.text = "Daily Leaderboard Score: "+DatabaseManager._instance.getDLS();
    }
    // public void loadDailyLeaderboardScore()
    // {
    //     Debug.Log("FETCHED DAILY SCORE");
    //     DatabaseManager._instance.getDailyLeaderboardScore(LeaderboardScore_userID_load.text,loadedScore=>
    //     {
    //         fetchedDailyLeaderboardScore.text = "SCORE IS : " + loadedScore;
    //     });
    // }
    
}
