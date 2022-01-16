using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataApi
{
    [Serializable]
    public class LeaderboardUser
    {
        public string assetID;
        public int sessionCounter;
        public string userName;
        public int userScore;
        public int userRank;
    }
    [Serializable]
    public class leaderboardObject
    {
        public List<LeaderboardUser> users;
    }
}
