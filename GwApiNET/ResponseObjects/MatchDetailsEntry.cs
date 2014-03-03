using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for match_details.json
    /// </summary>
    [Serializable]
    public class MatchDetailsEntry : ResponseObject
    {
        /// <summary>
        /// ID of the match
        /// </summary>
        [JsonProperty("match_id")]
        public string Id { get; set; }
        /// <summary>
        /// current scores
        /// </summary>
        [JsonProperty("scores")]
        public int[] Scores { get; set; }
        /// <summary>
        /// Red World Score
        /// </summary>
        public int RedScore
        {
            get { return Scores[0]; }
        }
        /// <summary>
        /// Blue World Score
        /// </summary>
        public int BlueScore
        {
            get { return Scores[1]; }
        }
        /// <summary>
        /// Green World Score
        /// </summary>
        public int GreenScore
        {
            get { return Scores[2]; }
        }

        /// <summary>
        /// Maps
        /// </summary>
        [JsonProperty("maps")]
        public EntryCollection<MatchMap> Maps { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MatchDetailsEntry()
        {
        }
    }

    [Serializable]
    public class MatchMap : ResponseObject
    {
        [JsonProperty("type")]
        public MatchMapType Type { get; set; }
        [JsonProperty("scores")]
        public int[] Scores { get; set; }
        /// <summary>
        /// Red World Score
        /// </summary>
        public int RedScore
        {
            get { return Scores[0]; }
        }
        /// <summary>
        /// Blue World Score
        /// </summary>
        public int BlueScore
        {
            get { return Scores[1]; }
        }
        /// <summary>
        /// Green World Score
        /// </summary>
        public int GreenScore
        {
            get { return Scores[2]; }
        }

        [JsonProperty("objectives")]
        public EntryCollection<MatchObjective> Objectives { get; set; }
    }

    [Serializable]
    public class MatchObjective : ResponseObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner")]
        public OwnerColor Owner { get; set; }
        [JsonProperty("owner_guild")]
        public Guid OwnerGuildId { get; set; }
    }
    
}
