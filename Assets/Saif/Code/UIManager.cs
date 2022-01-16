using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataApi;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public GameObject leaderboard;
    public GameObject userRankPrefab;

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnLeaderboard(leaderboardObject _leaderboardObject, string _leaderboardHeader)
    {
        if (GameObject.FindGameObjectWithTag("Leaderboard") != null)
            Destroy(GameObject.FindGameObjectWithTag("Leaderboard"));

        GameObject _SceneCanvas = GameObject.Find("Canvas");
        if (_SceneCanvas == null)
        {
            Debug.Log("CANT FIND CANVAS...");
            return;
        }
        GameObject _leaderboardSpawn = Instantiate(leaderboard, _SceneCanvas.transform);
        Text leaderboardHeader = _leaderboardSpawn.transform.GetChild(0).GetComponentInChildren<Text>();
        leaderboardHeader.text = _leaderboardHeader;
        Transform _leaderboardParent = _leaderboardSpawn.transform.GetChild(1);

        foreach (LeaderboardUser _user in _leaderboardObject.users)
        {
            GameObject _userRak = Instantiate(userRankPrefab, _leaderboardParent) as GameObject;
            //it is better to store values in their respective types, than convert to string, than storing as string than convert into something else! 
            _userRak.GetComponent<RankUser>().setRankInfo(_user.userName, _user.userScore.ToString(), _user.userRank.ToString());
        }
    }
}