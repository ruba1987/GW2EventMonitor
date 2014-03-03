using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Event Entry
    /// <remarks>Provides mapping between world_id, map_id, event_id and the <seealso cref="EventState"/></remarks>
    /// </summary>
    [Serializable]
    public class EventEntry : ResponseObject
    {
        [JsonProperty("world_id")]
        public int WorldId { get; set; }

        [JsonProperty("map_id")]
        public int MapId { get; set; }

        [JsonProperty("event_id")]
        public Guid EventId { get; set; }

        [JsonProperty("state")]
        public EventState State { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EventEntry()
        {
        }
    }

}
