using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for event_names.json
    /// </summary>
    [Serializable]
    public class EventNameEntry : ResponseObject
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EventNameEntry()
        {
        }
    }
}
