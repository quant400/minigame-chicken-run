
namespace leaderboardModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    using UnityEngine;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;



    [System.Serializable]
    public class assetClass
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("id")]
        public long TemperatureId { get; set; }

        [JsonProperty("dailyScore")]
        public long DailyScore { get; set; }

        [JsonProperty("allTimeScore")]
        public long AllTimeScore { get; set; }

        [JsonProperty("dailySessionPlayed")]
        public long DailySessionPlayed { get; set; }

        [JsonProperty("totalSessionPlayed")]
        public long TotalSessionPlayed { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("__v")]
        public long V { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    [System.Serializable]
    public class allTimeLEaderBoard
    {
        public List<assetClass> allTimeLeadboeardClass;
    }
}

