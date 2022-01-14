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

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        databaseRefrence = FirebaseDatabase.DefaultInstance.RootReference;   
    }

    public void setScoreInLeaderboard(string _userID,int newScore)
    {
        databaseRefrence
        .Child("Leaderboard")
        .Child("Users")
        .Child(_userID)
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
    public void setScoreInDailyLeaderboard(string _userID,int newScore)
    {
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(_userID)
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
    public void getDailyLeaderboardScore(string _userID,Action<long> result)
    {
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(_userID)
        .Child("userScore")
        .GetValueAsync().ContinueWithOnMainThread(task=>
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
            result((long)snapshot.Value);
        });
    }
    public void getLeaderboardScore(string _userID,Action<long> result)
    {
        databaseRefrence
        .Child("Leaderboard")
        .Child("Users")
        .Child(_userID)
        .Child("userScore")
        .GetValueAsync().ContinueWithOnMainThread(task=>
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
            result((long)snapshot.Value);
        });
    }
}
