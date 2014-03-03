using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for map_floor.json
    /// </summary>
    [Serializable]
    public class MapFloorEntry : ResponseObject
    {
        /// <summary>
        /// Texture dimensions in pixels for the floor of a map.
        /// </summary>
        [JsonProperty("texture_dims")]
        public int[] TextureDimensions { get; set; }
        /// <summary>
        /// Regions on the map.
        /// </summary>
        [JsonProperty("regions")]
        public Dictionary<int,RegionEntry> Regions { get; set; }
        /// <summary>
        /// Clamped view of the floor
        /// </summary>
        [JsonProperty("clamped_view")]
        public List<int[]> ClampedView { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapFloorEntry()
        {
        }
    }
    /// <summary>
    /// Map Floor Region
    /// </summary>
    [Serializable]
    public class RegionEntry : ResponseObject
    {
        /// <summary>
        /// Region ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the region.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Coordinate of the label (Name of the region).
        /// </summary>
        [JsonProperty("label_coord")]
        public int[] LabelCoord { get; set; }
        /// <summary>
        /// Maps of the region
        /// </summary>
        [JsonProperty("maps")]
        public SerializableDictionary<int, RegionMap> Maps { get; set; }

    }
    /// <summary>
    /// Map of a region
    /// </summary>
    [Serializable]
    public class RegionMap : MapEntryBase
    {
        /// <summary>
        /// Name of the map.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get { return MapName; } set { MapName = value; } }
        /// <summary>
        /// Points of Interest on the map.
        /// </summary>
        [JsonProperty("points_of_interest")]
        public EntryCollection<PointOfInterest> PointsOfInterest { get; set; }
        /// <summary>
        /// Tasks on the map.
        /// </summary>
        [JsonProperty("tasks")]
        public EntryCollection<TaskEntry> Tasks { get; set; }
        /// <summary>
        /// Skill Challenges for hte map.
        /// </summary>
        [JsonProperty("skill_challenges")]
        public EntryCollection<SkillChallengeEntry> SkillChallenges { get; set; }
        /// <summary>
        /// Sectors of the map.
        /// </summary>
        [JsonProperty("sectors")]
        public EntryCollection<SectorEntry> Sectors { get; set; }
    }
    /// <summary>
    /// Map Sector information
    /// </summary>
    [Serializable]
    public class SectorEntry : ResponseObject
    {
        /// <summary>
        /// Sector ID
        /// </summary>
        [JsonProperty("sector_id")]
        public int Id { get; set; }
        /// <summary>
        /// Sector Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Sector Level
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }
        /// <summary>
        /// Sector Coordinates
        /// </summary>
        /// <remarks>pixel values</remarks>
        [JsonProperty("coord")]
        public double[] Coordinates { get; set; }

    }
    /// <summary>
    /// Skill Challenge Information
    /// </summary>
    [Serializable]
    public class SkillChallengeEntry : ResponseObject
    {
        /// <summary>
        /// Skill Challenge Location
        /// </summary>
        /// <remarks>pixel values</remarks>
        [JsonProperty("coord")]
        public double[] Coordinents { get; set; }
    }
    /// <summary>
    /// Map Task Entry
    /// </summary>
    [Serializable]
    public class TaskEntry : ResponseObject
    {
        /// <summary>
        /// Task ID
        /// </summary>
        [JsonProperty("task_id")]
        public int Id { get; set; }
        /// <summary>
        /// Task Objective
        /// </summary>
        [JsonProperty("objective")]
        public string Objective { get; set; }
        /// <summary>
        /// Task Level
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }
        /// <summary>
        /// Task Location
        /// </summary>
        /// <remarks>pixel values</remarks>
        [JsonProperty("coord")]
        public double[] Coordinates { get; set; }
    }
    /// <summary>
    /// Map Point of Interest
    /// </summary>
    [Serializable]
    public class PointOfInterest : ResponseObject
    {
        /// <summary>
        /// Point of Interest ID
        /// </summary>
        [JsonProperty("poi_id")]
        public int Id { get; set; }
        /// <summary>
        /// Point of Interest Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Type of Point of Interest
        /// </summary>
        [JsonProperty("type")]
        public PointOfInterestType Type { get; set; }
        /// <summary>
        /// Floor location
        /// </summary>
        [JsonProperty("floor")]
        public int Floor { get; set; }
        /// <summary>
        /// Point of Interest Location
        /// <remarks>pixel values</remarks>
        /// </summary>
        [JsonProperty("coord")]
        public double[] Coordinates { get; set; }

    }

}
