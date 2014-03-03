using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Base class for Map Entries.
    /// </summary>
    [Serializable]
    public abstract class MapEntryBase : ResponseObject
    {
        /// <summary>
        /// Map ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Map Name
        /// </summary>
        [JsonProperty("map_name")]
        public string MapName { get; set; }
        /// <summary>
        /// Minimum Map Level.
        /// </summary>
        [JsonProperty("min_level")]
        public int MinLevel { get; set; }
        /// <summary>
        /// Maximum map level
        /// </summary>
        [JsonProperty("max_level")]
        public int MaxLevel { get; set; }
        /// <summary>
        /// Default floor for the map.
        /// </summary>
        [JsonProperty("default_floor")]
        public int DefaultFloor { get; set; }
        /// <summary>
        /// ID of the region the map is in
        /// </summary>
        [JsonProperty("region_id")]
        public int RegionId { get; set; }
        /// <summary>
        /// Name of the region the map is in.
        /// </summary>
        [JsonProperty("region_name")]
        public string RegionName { get; set; }
        /// <summary>
        /// Map boundries.
        /// <remarks>These boundries build a rectangle and provides a way to project a players position to pixel values on the continent.
        /// A maps coordinate system assumes the origin (0,0) of the map is near the center of the map.
        /// See <seealso cref="GwMapsHelper"/> for functions on converting a players position to pixel coordinates for a particular map.</remarks>
        /// </summary>
        [JsonProperty("map_rect")]
        public List<int[]> MapRectangle { get; set; }
        /// <summary>
        /// Pixel coordinates that bounds a rectangle area of the map.  This is used to create a project to and from player coordinates when used with <seealso cref="MapRectangle"/>.
        /// See <seealso cref="GwMapsHelper"/> for functions on converting a players position to pixel coordinates for a particular map.</remarks>
        /// </summary>
        [JsonProperty("continent_rect")]
        public List<int[]> ContinentRectangle { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapEntryBase()
        {
        }
    }
}
