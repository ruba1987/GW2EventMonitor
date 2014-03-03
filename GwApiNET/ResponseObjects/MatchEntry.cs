using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Match entry in matches.json
    /// </summary>
    [Serializable]
    public class MatchEntry : ResponseObject
    {
        /// <summary>
        /// ID of match
        /// </summary>
        [JsonProperty("wvw_match_id")]
        public string Id { get; set; }
        /// <summary>
        /// Red Team World ID
        /// </summary>
        [JsonProperty("red_world_id")]
        public int RedWorldId { get; set; }
/// <summary>
/// Blue Team world ID
/// </summary>
        [JsonProperty("blue_world_id")]
        public int BlueWorldId { get; set; }
        /// <summary>
        /// Green Team world ID
        /// </summary>
        [JsonProperty("green_world_id")]
        public int GreenWorldId { get; set; }
        /// <summary>
        /// Starting Time of the match.
        /// </summary>
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Ending time of the match.
        /// <remarks>This may be in the future if the match has not yet ended.</remarks>
        /// </summary>
        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MatchEntry()
        {
        }
    }
}
