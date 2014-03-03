using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// entry in objective_names.json
    /// </summary>
    [Serializable]
    public class ObjectiveNameEntry : ResponseObject
    {
        /// <summary>
        /// Objective ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// Objective Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ObjectiveNameEntry()
        {
        }
    }
}
