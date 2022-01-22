using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataApi;
using UnityEngine.Networking;
using Tamarin.Common;
using Tamarin.FirebaseX;
using UniRx.Triggers;
using UniRx;
using UniRx.Operators;
using System.Text;

public class DatabaseManagerRestApi : MonoBehaviour
{
    public static DatabaseManagerRestApi _instance;
    FirebaseAPI firebase;
    ReactiveProperty<int> sessionCounterReactive = new ReactiveProperty<int>();
    int localID;
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
            if((int)ss <= 10)
            {
                getLeaderboardScore(_assetID,res=>
                {
                    setScoreInLeaderboard(_assetID,_FighterName,_score + (((int)res) == -1 ? 0 : (int)res));
                });
                getDailyLeaderboardScore(_assetID,res=>
                {
                    setScoreInDailyLeaderboard(_assetID,_FighterName,(int)ss,_score + (((int)res) == -1 ? 0 : (int)res));
                });
            }else
                Debug.Log("REACHED DAILY SESSIONS LIMIT...");
        });
        
    }
    public void setScoreRestApiMain(string _assetID, int _score)
    {
        int id = int.Parse(_assetID);
        setScoreWithRestApi(id, _score);
    }
    public void setScoreWithRestApi(int assetID,int score)
    {
        StartCoroutine(getSessionCounterAndSetScoreFromApi("https://api.cryptofightclub.io/game/sdk/chicken/score",assetID,score));

    }
    public IEnumerator getSessionCounterAndSetScoreFromApi(string url,int assetId, int score)
    {
        leaderboardModel.userGetDataModel idData = new leaderboardModel.userGetDataModel();
        idData.id = assetId;
        localID = assetId;
        string idJsonData = JsonUtility.ToJson(idData);
        using (UnityWebRequest request = UnityWebRequest.Post(url.ToString(), idJsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(idJsonData) ? null : Encoding.UTF8.GetBytes(idJsonData));
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.error == null)
            {

                checkSessionCounter(Encoding.UTF8.GetString(request.downloadHandler.data));


                setScoreInLeaderBoard(score);
                Debug.Log(Encoding.UTF8.GetString(request.downloadHandler.data));

            }
            else
            {
                Debug.Log("error in server");
            }


        }



    }
    public void checkSessionCounter(string url)
    {

        string MatchData = url;
        Debug.Log(MatchData);
        leaderboardModel.assetClass playerData = restApiDataView.JsonUtil.fromJson<leaderboardModel.assetClass>(url);
        if (playerData != null)
        {
            sessionCounterReactive.Value = playerData.dailySessionPlayed;
            SingleplayerGameControler.instance.dailyScore = playerData.dailyScore;
            SingleplayerGameControler.instance.sessions = playerData.dailySessionPlayed;
            SingleplayerGameControler.instance.AlltimeScore = playerData.allTimeScore;
            SingleplayerGameControler.instance.dailysessionReactive.Value = playerData.dailySessionPlayed;



        }



    }
    public void setScoreInLeaderBoard(int scoreAdded)
    {
        if (sessionCounterReactive.Value < 10)
        {
            StartCoroutine(setScoreInLeaderBoeardRestApi(scoreAdded));
        }
        else
        {
            Debug.Log("you reach daily Limits");
        }
    }
    public IEnumerator setScoreInLeaderBoeardRestApi( int scoreAdded)
    {
         leaderboardModel.userPostedData postedData = new leaderboardModel.userPostedData();
        postedData.id = localID;
        postedData.score = scoreAdded;
        string idJsonData = JsonUtility.ToJson(postedData);

        using (UnityWebRequest request = UnityWebRequest.Post("https://api.cryptofightclub.io/game/sdk/chicken/increment", idJsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(idJsonData) ? null : Encoding.UTF8.GetBytes(idJsonData));
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.error == null)
            {




                Debug.Log(Encoding.UTF8.GetString(request.downloadHandler.data));

            }
            else
            {
                Debug.Log("error in server");
            }


        }



    }

    public void getDataFromRestApi(int assetId)
    {
        StartCoroutine(getDataRestApi(assetId));
    }
    public IEnumerator getDataRestApi(int assetId)
    {
        leaderboardModel.userGetDataModel idData = new leaderboardModel.userGetDataModel();
        idData.id = assetId;
        localID = assetId;
        string idJsonData = JsonUtility.ToJson(idData);
        Debug.Log(idData);
        using (UnityWebRequest request = UnityWebRequest.Put("https://api.cryptofightclub.io/game/sdk/chicken/score", idJsonData))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(idJsonData);
            request.method = "POST";
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                string result = Encoding.UTF8.GetString(request.downloadHandler.data);

                checkSessionCounter(result);


                Debug.Log(request.downloadHandler.text);

            }
            else
            {
                Debug.Log("error in server");
            }


        }
    }
    public void SetDemoScore(string _assetID, string _FighterName, int _score)
    {
        SetDemoScoreInLeaderboard(_assetID, _FighterName, _score);
        SetDemoScoreInDailyLeaderboard(_assetID, _FighterName, 1, _score);
    }
    public async void SetDemoScoreInLeaderboard(string _assetID, string _name, int newScore)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        LeaderboardUser _user = new LeaderboardUser(_assetID, _name, 1, newScore);
        await firebase.database.SetRawAsync($"Leaderboard/Assets/{_assetID}", _user);
        Debug.Log("SCORE STORED IN LEADERBOARD!");
    }
    public async void SetDemoScoreInDailyLeaderboard(string _assetID, string _name, int _sessionCounter, int newScore)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        LeaderboardUser _user = new LeaderboardUser(_assetID, _name, 1, newScore);
        await firebase.database.SetRawAsync($"DailyLeaderboard/Assets/{_assetID}", _user);
        increaseSessionCounter(_assetID);
        Debug.Log("SCORE STORED IN Daily LEADERBOARD!");
    }

    public async void setScoreInLeaderboard(string _assetID,string _name,int newScore)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        LeaderboardUser _user = new LeaderboardUser(_assetID,_name,0,newScore);
        await firebase.database.SetRawAsync($"Leaderboard/Assets/{_assetID}", _user);
        Debug.Log("SCORE STORED IN LEADERBOARD!");
    
    }
    public async void setScoreInDailyLeaderboard(string _assetID,string _name,int _sessionCounter,int newScore)
    {
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        LeaderboardUser _user = new LeaderboardUser(_assetID,_name,_sessionCounter,newScore);
        await firebase.database.SetRawAsync($"DailyLeaderboard/Assets/{_assetID}", _user);
        increaseSessionCounter(_assetID);
        Debug.Log("SCORE STORED IN Daily LEADERBOARD!");

        
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
