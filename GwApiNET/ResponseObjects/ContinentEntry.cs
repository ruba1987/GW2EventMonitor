using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for continents.json
    /// </summary>
    [Serializable]
    public class ContinentEntry : ResponseObject
    {
        /// <summary>
        /// ID of continent
        /// </summary>
        [JsonProperty("continents")]
        public int Id { get; set; }
        /// <summary>
        /// Name of continent
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Pixel dimensions of continent
        /// </summary>
        [JsonProperty("continent_dims")]
        public int[] Dimension { get; set; }
        /// <summary>
        /// Minimum zoom level.  Used with world tile services to determine what tiles are available at a given resolution for the continent.
        /// </summary>
        [JsonProperty("min_zoom")]
        public int MinZoom { get; set; }
        /// <summary>
        /// Maximum zoom level.  Used with world tile services to determine what tiles are available at a given resolution for the continent.
        /// </summary>
        [JsonProperty("max_zoom")]
        public int MaxZoom { get; set; }
        /// <summary>
        /// List of floors on the continent.
        /// </summary>
        [JsonProperty("floors")]
        public int[] Floors { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ContinentEntry()
        {
        }

    }
}
