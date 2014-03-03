using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for map_names.json
    /// </summary>
    [Serializable]
    public class MapNameEntry : ResponseObject
    {
        /// <summary>
        /// ID of map
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// Name of map.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapNameEntry()
        {
        }
    }
}
