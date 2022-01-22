using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataApi;
using Tamarin.Common;
using Tamarin.FirebaseX;
public class LeaderBoardControllerRestApi : MonoBehaviour
{
    [SerializeField]
    GameObject leaderBoard;
    [SerializeField]
    GameObject leaderboardEntryPrefab;
    [SerializeField]
    Transform layoutGroup;
    FirebaseAPI firebase;
    restApiDataView restApiView;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        restApiView = GetComponent<restApiDataView>();
    }
    private async void Start()
    {
        //here we are referencing the api, to make a shorthand for firebase. (cause we are lazy devs, and Firebase.Instance is too long to write every time :))
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        firebase = FirebaseAPI.Instance;
    }

    public void ToggleLeaderBoard(bool b)
    {
        if (b)
        {
            leaderBoard.SetActive(true);
            restApiView.DisplayDailyLeaderboardRestApi();
            leaderBoard.GetComponent<LeaderBoardScript>().Activate();
        }
        else
        {
            leaderBoard.GetComponent<LeaderBoardScript>().Deactivate();

        }
    }

    public async void UpDateLeaderBoardDaily()
    {
        Clean();

        var query = new Dictionary<string, object>();
        query.Add("NumResults", 15);
        leaderboardObject _Leaderboard = await firebase.functions.HttpsCall<leaderboardObject>("getDailyLeaderboard", query);
        Clean();
        foreach (LeaderboardUser _user in _Leaderboard.users)
        {
            if (_user.userScore > 0)
            {
                var temp = Instantiate(leaderboardEntryPrefab, layoutGroup);
                temp.GetComponent<LeaderBoardEntry>().Set(_user.userRank.ToString(), _user.userName, _user.assetID, _user.userScore.ToString());
            }
        }
    }
   
    public async void UpDateLeaderBoardMonthly()
    {
        Clean();

        var query = new Dictionary<string, object>();
        query.Add("NumResults", 15);
        leaderboardObject _Leaderboard = await firebase.functions.HttpsCall<leaderboardObject>("getLeaderboard", query);
        Clean();
        foreach (LeaderboardUser _user in _Leaderboard.users)
        {
            if (_user.userScore > 0)
            {
                var temp = Instantiate(leaderboardEntryPrefab, layoutGroup);
                temp.GetComponent<LeaderBoardEntry>().Set(_user.userRank.ToString(), _user.userName, _user.assetID, _user.userScore.ToString());
            }
           
        }
    }
    public void UpDateLeaderBoardDailyRestApi(leaderboardModel.assetClass[] _leaderboardObject, string _leaderboardHeader)
    {
        Clean();

        var query = new Dictionary<string, object>();
        query.Add("NumResults", 15);
        Clean();
        int score;
        int rank = 1;
        foreach (leaderboardModel.assetClass _user in _leaderboardObject)
        {
            if (_leaderboardHeader == "Daily LEADERBOARD")
            {
                score = _user.dailyScore;
            }
            else
            {
                score = _user.allTimeScore;
            }
            if (_user.dailyScore > 0)
            {
                var temp = Instantiate(leaderboardEntryPrefab, layoutGroup);
                temp.GetComponent<LeaderBoardEntry>().Set(rank.ToString(), _user.name, _user.id.ToString(), _user.dailyScore.ToString());
            }
            rank++;
        }
    }
    public void UpDateLeaderBoardAllTimeRestApi(leaderboardModel.assetClass[] _leaderboardObject, string _leaderboardHeader)
    {
        Clean();

        var query = new Dictionary<string, object>();
        query.Add("NumResults", 15);
        Clean();
        int score;
        int rank = 1;
        foreach (leaderboardModel.assetClass _user in _leaderboardObject)
        {
            if (_leaderboardHeader == "Daily LEADERBOARD")
            {
                score = _user.dailyScore;
            }
            else
            {
                score = _user.allTimeScore;
            }
            if (_user.dailyScore > 0)
            {
                var temp = Instantiate(leaderboardEntryPrefab, layoutGroup);
                temp.GetComponent<LeaderBoardEntry>().Set(rank.ToString(), _user.name, _user.id.ToString(), _user.dailyScore.ToString());
            }
            rank++;
        }
    }
    void Clean()
    {
        for (int i = layoutGroup.childCount - 1; i >= 0; i--)
        {
            Destroy(layoutGroup.GetChild(i).gameObject);
        }
    }
}
