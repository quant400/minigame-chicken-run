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

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.X))
        //     // registerUser("CUSTOM USER IDD","ASSET ID","NAME");

    }

    public async void registerUser(string assetID,string userName)
    {
        // await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        // var _user = new LeaderboardUser(assetID, userName,0,0,0);
        // //As this is the only function in this class, we are not referencing the API, as its a singleton we can acces it from anywhere! 
        // //one can acces child nodes in the realtimedb (JSONTREE) by seperating childs with /
        // await FirebaseAPI.Instance.database.SetRawAsync($"Leaderboard/Users/{PlayerPrefs.GetString("UserID","0000ERRROR")}", _user);
        // await FirebaseAPI.Instance.database.SetRawAsync($"DailyLeaderboard/Users/{PlayerPrefs.GetString("UserID","0000ERRROR")}", _user);

        // Debug.Log("USER REGISTERED SUCCESSFULLY");
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