using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataApi
{
    [Serializable]
    public class LeaderboardUser
    {
        public string userID;
        public string userName;
        public string userScore;
        public string userRank;
    }
    [Serializable]
    public class leaderboardObject
    {
        public List<LeaderboardUser> users;
    }
}
