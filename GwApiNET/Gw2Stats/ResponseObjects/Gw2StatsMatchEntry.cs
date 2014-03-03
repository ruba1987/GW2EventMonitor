using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using Newtonsoft.Json;

namespace GwApiNET.Gw2Stats.ResponseObjects
{

    [Serializable]
    public class Gw2StatsMatchEntry : ResponseObject
    {

        [JsonProperty("retrieve_time")]
        public DateTime RetriveTime { get; set; }

        public EntryDictionary<string, IList<MatchRegion>> Region { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsMatchEntry()
        {
        }
    }

    [Serializable]
    public class MatchRegion
    {
        public string RegionId { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("last_update")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("match_id")]
        public string MatchId { get; set; }

        [JsonProperty("unique_id")]
        public Guid UniqueId { get; set; }

        [JsonProperty("worlds")]
        internal IList<MatchWorld> _Worlds { get; set; }

        public IDictionary<int, MatchWorld> Worlds { get; set; }
    }

    [Serializable]
    public class MatchWorld
    {
        [JsonProperty("world_id")]
        public int Id { get; set; }

        public string Name { get; set; }
        public OwnerColor Color { get; set; }
        public int Score { get; set; }
        public int PPT { get; set; }
        public MatchObjectives Objectives { get; set; }
        public Gw2StatsRatingsData Rating { get; set; }
    }

    [Serializable]
    public class MatchObjectives
    {
        public int Camps { get; set; }
        public int Towers { get; set; }
        public int Keeps { get; set; }
        public int Castles { get; set; }
    }
}
