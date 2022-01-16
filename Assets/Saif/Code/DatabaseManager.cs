using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using Firebase.Extensions;
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager _instance;
    DatabaseReference databaseRefrence;
    long SessionCounter;
    long leaderboardScore;
    long dailyleaderboardScore;

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        databaseRefrence = FirebaseDatabase.DefaultInstance.RootReference;
        InvokeRepeating("getSessionCounter",1f,2f);
        InvokeRepeating("getLeaderboardScore",1f,2f);
        InvokeRepeating("getDailyLeaderboardScore",1f,2f);
    }
    public void setScore(int _score)
    {
        setScoreInDailyLeaderboard(_score+(int)dailyleaderboardScore);
        setScoreInLeaderboard(_score+(int)leaderboardScore);
    }
    void setScoreInLeaderboard(int newScore)
    {
        databaseRefrence
        .Child("Leaderboard")
        .Child("Users")
        .Child(AuthManager._instance.getcurrentUser().UserId)
        .Child("userScore")
        .SetValueAsync(newScore)
        .ContinueWithOnMainThread(task=>
        {
            if(task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if(task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            Debug.Log("SCORE STORED IN LEADERBOARD!");   
        });
    }
    public void setScoreInDailyLeaderboard(int newScore)
    {
        if(SessionCounter >= 10)
            return;
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(AuthManager._instance.getcurrentUser().UserId)
        .Child("userScore")
        .SetValueAsync(newScore)
        .ContinueWithOnMainThread(task=>
        {
            if(task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if(task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            Debug.Log("SCORE STORED IN DAILY LEADERBOARD!");
            IncremeantSessionCounter();   
        });
    }
    public void getDailyLeaderboardScore()
    {
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(AuthManager._instance.getcurrentUser().UserId)
        .Child("userScore")
        .GetValueAsync().ContinueWith(task=>
        {
            if(task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if(task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            DataSnapshot snapshot = task.Result;
            dailyleaderboardScore = (long)snapshot.Value;
            // Debug.Log("DAILY LEADERBOARD SCORE IS : " + dailyleaderboardScore);
            // result((int)snapshot.Value);
        });
    }
    public void getLeaderboardScore()
    {
        databaseRefrence
        .Child("Leaderboard")
        .Child("Users")
        .Child(AuthManager._instance.getcurrentUser().UserId)
        .Child("userScore")
        .GetValueAsync().ContinueWith(task=>
        {
            if(task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if(task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            DataSnapshot snapshot = task.Result;
            leaderboardScore = (long)snapshot.Value;
            // Debug.Log("LEADERBOARD SCORE IS : " + leaderboardScore);
            // result((int)snapshot.Value);
        });
    }

    public void getSessionCounter()
    {
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(AuthManager._instance.getcurrentUser().UserId)
        .Child("sessionCounter")
        .GetValueAsync().ContinueWith(task=>
        {
            if(task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if(task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            DataSnapshot snapshot = task.Result;
            Debug.Log("CURRENT SESSION COUNTER : " + SessionCounter);
            SessionCounter = (long)snapshot.Value;
        });
    }
    public void IncremeantSessionCounter()
    {
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(AuthManager._instance.getcurrentUser().UserId)
        .Child("sessionCounter")
        .SetValueAsync(SessionCounter+1)
        .ContinueWithOnMainThread(task=>
        {
            if(task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if(task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            Debug.Log("Counter Increased");   
        });
    }
    public long getSessionsCounter()
    {
        return SessionCounter;
    }
    public long getLS()
    {
        return leaderboardScore;
    }
    public long getDLS()
    {
        return dailyleaderboardScore;
    }
}
