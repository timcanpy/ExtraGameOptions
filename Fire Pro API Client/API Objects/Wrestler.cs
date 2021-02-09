using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePro_API.Models
{
    public class Wrestler
    {
        #region Properties
        public string ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }

        [JsonProperty("Country")]
        public string Region { get; set; }
        public int MoraleRank { get; set; }
        public int MoralePoints { get; set; }
        public int MatchCount { get; set; }
        public decimal AverageRating { get; set; }
        [JsonProperty("CurrentWins")]
        public int Wins { get; set; }
        [JsonProperty("CurrentLosses")]
        public int Losses { get; set; }
        [JsonProperty("CurrentDraws")]
        public int Draws { get; set; }
        public string CurrentPromotion { get; set; }
        #endregion
    }
}
