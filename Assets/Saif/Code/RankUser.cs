using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankUser : MonoBehaviour
{
    public Text userName;
    public Text userScore;
    public Text userRank;

    public void setRankInfo(string _name, string _score, string _rank)
    {
        userName.text = _name;
        userScore.text = _score;
        userRank.text = _rank;
    }
}