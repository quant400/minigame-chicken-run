using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataApi;


using Tamarin.Common;
using Tamarin.FirebaseX;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager _instance;
    FirebaseAPI firebase;

    private void Awake()
    {
        _instance = this;
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.Space))
        //     setScore("00256454","THE RED FIGHTER",200);
    }

    //Most of the calls in the api are async calls, which makes code clean and tidy, and powerful! 
    //async calls are running on threads (depending on the platform) therefore it is advised to avoid threadhopping, and editor events (like button clicks)
    //if not familiar with async/await go and have a read on it! it is awesome :) 
    private async void Start()
    {
        //here we are referencing the api, to make a shorthand for firebase. (cause we are lazy devs, and Firebase.Instance is too long to write every time :))
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        firebase = FirebaseAPI.Instance;
    }
    public void setScore(string _assetID,string _FighterName,int _score)
    {
        getSessionsCounter(_assetID,ss=>
        {
            if((int)ss < 10)
            {
                getLeaderboardScore(_assetID,res=>
                {
                    setScoreInLeaderboard(_assetID,_FighterName,_score + (int)res);
                });
                getDailyLeaderboardScore(_assetID,res=>
                {
                    setScoreInDailyLeaderboard(_assetID,_FighterName,(int)ss,_score + (int)res);
                });
            }else
                Debug.Log("REACHED DAILY SESSIONS LIMIT...");
        });
        
    }
    public async void setScoreInLeaderboard(string _assetID,string _name,int newScore)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        LeaderboardUser _user = new LeaderboardUser(_assetID,_name,0,newScore);
        await firebase.database.SetRawAsync($"Leaderboard/Assets/{_assetID}", _user);
        Debug.Log("SCORE STORED IN LEADERBOARD!");
        /*
        databaseRefrence
        .Child("Leaderboard")
        .Child("Users")
        .Child(_userID)
        .Child("userScore")
        .SetValueAsync(newScore)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if (task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            Debug.Log("SCORE STORED IN LEADERBOARD!");
        });
        */
    }
    public async void setScoreInDailyLeaderboard(string _assetID,string _name,int _sessionCounter,int newScore)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        LeaderboardUser _user = new LeaderboardUser(_assetID,_name,_sessionCounter,newScore);
        await firebase.database.SetRawAsync($"DailyLeaderboard/Assets/{_assetID}", _user);
        increaseSessionCounter(_assetID);
        Debug.Log("SCORE STORED IN Daily LEADERBOARD!");

        /*
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(_userID)
        .Child("userScore")
        .SetValueAsync(newScore)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("TASK FAULTED");
                return;
            }
            else if (task.IsCanceled)
            {
                Debug.Log("TASK CANCELED");
                return;
            }
            Debug.Log("SCORE STORED IN LEADERBOARD!");
        });
        */
    }
    public async void getDailyLeaderboardScore(string _assetID,Action<long> result)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        //we store the userScore as a long, so we ask the api to return a long type, the type here can be any type which represents the result.
        //eg: it could be QueryAsync<LeaderboardUser>($"DailyLeaderboard/Users/{_userID}") to retrieve the whole user. 
        //eg: or QueryAsync<Dictionary<string, LeaderboardUser>>($"DailyLeaderboard/Users") to retreive the whole list of Users
        var score = await firebase.database.QueryAsync<long>($"DailyLeaderboard/Assets/{_assetID}/userScore");
        result(score);

        /*
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
        */
    }
    public async void getLeaderboardScore(string _assetID,Action<long> result)
    {
        var score = await firebase.database.QueryAsync<long>($"Leaderboard/Assets/{_assetID}/userScore");
        result(score);
        /*
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
        */
    }
    public async void getSessionsCounter(string _assetID,Action<long> result)
    {
        var score = await firebase.database.QueryAsync<long>($"DailyLeaderboard/Assets/{_assetID}/sessionCounter");
        PlayerPrefs.SetInt("sessionCounter",(int)score);
        result(score);
    }
    public async void increaseSessionCounter(string _assetID)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        await firebase.database.SetAsync($"DailyLeaderboard/Assets/{_assetID}/sessionCounter", PlayerPrefs.GetInt("sessionCounter",0)+1);
    }

}
