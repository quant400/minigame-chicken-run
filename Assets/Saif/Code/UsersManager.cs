using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataApi;
using Firebase;
using Firebase.Database;
public class UsersManager : MonoBehaviour
{
    public static UsersManager _instance;
    DatabaseReference databaseRefrence;
    private void Awake() {
        _instance = this;
    }

    private void Start() {
        databaseRefrence = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
    public void registerUser(string _userName,string _userID)
    {
        LeaderboardUser _user = new LeaderboardUser()
        {
            userID = _userID,
            userName = _userName,
            userRank = "0",
            userScore = "0"
        };
        string jsonUser = JsonUtility.ToJson(_user);
        databaseRefrence
        .Child("Leaderboard")
        .Child("Users")
        .Child(_userID)
        .SetRawJsonValueAsync(jsonUser).ContinueWith(task=>
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
            Debug.Log("USER REGISTERED SUCCESSFULLY");
        });
   
        databaseRefrence
        .Child("DailyLeaderboard")
        .Child("Users")
        .Child(_userID)
        .SetRawJsonValueAsync(jsonUser).ContinueWith(task=>
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
            Debug.Log("USER REGISTERED SUCCESSFULLY");
        });
    }

}
