using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// World Name entry for world_names.json object
    /// </summary>
    [Serializable]
    public class WorldNameEntry : ResponseObject
    {
        /// <summary>
        /// World ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// World Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public WorldNameEntry()
        {
        }
    }
}
