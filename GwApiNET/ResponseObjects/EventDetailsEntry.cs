using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for event_details.json
    /// </summary>
    [Serializable]
    public class EventDetailsEntry : ResponseObject
    {
        /// <summary>
        /// Event ID
        /// </summary>
        [JsonProperty("event_id")]
        public Guid EventId { get; set; }
        /// <summary>
        /// Name of Event
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Event Level
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }
        /// <summary>
        /// ID of the map the event is on.
        /// </summary>
        [JsonProperty("map_id")]
        public int MapId { get; set; }
        /// <summary>
        /// Event flags.
        /// </summary>
        [JsonProperty("flags")]
        public FlagType[] Flags { get; set; }
        /// <summary>
        /// Event Location
        /// </summary>
        [JsonProperty("location")]
        public EventLocation Location { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EventDetailsEntry()
        {
        }

        public enum FlagType
        {
            group_event,
            map_wide,
        }
    }
    /// <summary>
    /// Event Location information
    /// </summary>
    [Serializable]
    public class EventLocation
    {
        /// <summary>
        /// Location Type
        /// </summary>
        [JsonProperty("type")]
        public EventLocationType Type { get; set; }
        /// <summary>
        /// Center of event coordinates
        /// <remarks>Uses world/player coordinates.  This can be translated to pixel coordinates using <seealso cref="GwMapsHelper"/>.  Use the MapID to determine the MapRectangle boundries the event is on.</remarks>
        /// </summary>
        [JsonProperty("center")]
        public double[] Center { get; set; }
        /// <summary>
        /// Radius of event using world coordinates
        /// </summary>
        [JsonProperty("radius")]
        public double Radius { get; set; }
        /// <summary>
        /// Rotation
        /// </summary>
        [JsonProperty("rotation")]
        public double Rotation { get; set; }
        /// <summary>
        /// Event Points
        /// </summary>
        [JsonProperty("points")]
        public List<double[]> Points { get; set; }
        /// <summary>
        /// Z-Range
        /// </summary>
        [JsonProperty("z_range")]
        public int[] ZRange { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        [JsonProperty("height")]
        public double Height { get; set; }
    }
    /// <summary>
    /// Event Location Type.
    /// </summary>
    public enum EventLocationType
    {
        /// <summary>
        /// Event boundries form a sphere
        /// </summary>
        Sphere,
        /// <summary>
        /// Event boundries form a polygon
        /// </summary>
        Poly,
        /// <summary>
        /// event boundries form a cylinder
        /// </summary>
        Cylinder
    }
}
