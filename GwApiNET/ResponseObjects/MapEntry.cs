using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for maps.json
    /// </summary>
    [Serializable]
    public class MapEntry : MapEntryBase
    {
        /// <summary>
        /// List of Floors for the map
        /// </summary>
        [JsonProperty("floors")]
        public int[] Floors { get; set; }
        /// <summary>
        /// Continent ID the map is on
        /// </summary>
        [JsonProperty("continent_id")]
        public int ContinentId { get; set; }
        /// <summary>
        /// Continent name the map is on.
        /// </summary>
        [JsonProperty("continent_name")]
        public string ContinentName { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapEntry()
        {
        }
    }
}
