using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataApi;
using UnityEngine.Networking;
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager _instance;
    public GameObject loadingIconPrefab;
    GameObject _spawnedLoadingIcon;

    private void Awake() {
        _instance = this;
    }
    public void DisplayLeaderboard()
    {
        _spawnedLoadingIcon = Instantiate(loadingIconPrefab,GameObject.Find("Canvas").transform);
        StartCoroutine(fetchLeaderboardData());
    }
    public void DisplayDailyLeaderboard()
    {
        _spawnedLoadingIcon = Instantiate(loadingIconPrefab,GameObject.Find("Canvas").transform);
        StartCoroutine(fetchDailyLeaderboardData());
    }
    IEnumerator fetchLeaderboardData()
    {
        string leaderboardFunc = "https://us-central1-cfc-servers-333812.cloudfunctions.net/getLeaderboard?NumResults=10";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(leaderboardFunc))
        {
            yield return webRequest.SendWebRequest();
            if((webRequest.result == UnityWebRequest.Result.ConnectionError) || (webRequest.result == UnityWebRequest.Result.ProtocolError))
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                response = "{\"users\":" + response.ToString() + "}";
                leaderboardObject _Leaderboard = JsonUtility.FromJson<leaderboardObject>(response);
                Destroy(_spawnedLoadingIcon);
                UIManager._instance.SpawnLeaderboard(_Leaderboard,"LEADERBOARD");
            }
        }  
    }
    IEnumerator fetchDailyLeaderboardData()
    {
        string leaderboardFunc = "https://us-central1-cfc-servers-333812.cloudfunctions.net/getDailyLeaderboard?NumResults=10";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(leaderboardFunc))
        {
            yield return webRequest.SendWebRequest();
            if((webRequest.result == UnityWebRequest.Result.ConnectionError) || (webRequest.result == UnityWebRequest.Result.ProtocolError))
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                response = "{\"users\":" + response.ToString() + "}";
                leaderboardObject _Leaderboard = JsonUtility.FromJson<leaderboardObject>(response);
                Destroy(_spawnedLoadingIcon);
                UIManager._instance.SpawnLeaderboard(_Leaderboard,"Daily LEADERBOARD");
            }
        }  
    } 
}


