using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataApi;

using Tamarin.Common;
using Tamarin.FirebaseX;

public class UsersManager : MonoBehaviour
{
    public static UsersManager _instance;
    private void Awake()
    {
        _instance = this;
    }

    public async void registerUser(string _userName, string _userID)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        var _user = new LeaderboardUser(_userID, _userName, 0, 0);

        //As this is the only function in this class, we are not referencing the API, as its a singleton we can acces it from anywhere! 
        //one can acces child nodes in the realtimedb (JSONTREE) by seperating childs with /
        await FirebaseAPI.Instance.database.SetRawAsync($"Leaderboard/Users/{_userID}", _user);
        await FirebaseAPI.Instance.database.SetRawAsync($"DailyLeaderboard/Users/{_userID}", _user);

        Debug.Log("USER REGISTERED SUCCESSFULLY");
        /*
                string jsonUser = JsonUtility.ToJson(_user);
                databaseRefrence
                .Child("Leaderboard")
                .Child("Users")
                .Child(_userID)
                .SetRawJsonValueAsync(jsonUser).ContinueWith(task =>
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
                    Debug.Log("USER REGISTERED SUCCESSFULLY");
                });

                databaseRefrence
                .Child("DailyLeaderboard")
                .Child("Users")
                .Child(_userID)
                .SetRawJsonValueAsync(jsonUser).ContinueWith(task =>
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
                    Debug.Log("USER REGISTERED SUCCESSFULLY");
                });
                */
    }

}