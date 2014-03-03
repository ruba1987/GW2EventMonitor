using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for guild_details.json
    /// </summary>
    [Serializable]
    public class GuildDetailsEntry : ResponseObject
    {
        /// <summary>
        /// Guild ID
        /// </summary>
        [JsonProperty("guild_id")]
        public Guid GuildId { get; set; }
        //public string GuildIdString { get { return GuildId.ToString(); } set { GuildId = new Guid(value); } }
        /// <summary>
        /// Guild Name
        /// </summary>
        [JsonProperty("guild_name")]
        public string GuildName { get; set; }
        /// <summary>
        /// Guild TAG
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }
        /// <summary>
        /// Guild Emblem
        /// </summary>
        [JsonProperty("emblem")]
        public EmblemProperties Emblem { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public GuildDetailsEntry()
        {
        }

        /// <summary>
        /// Guild Emblem properties
        /// </summary>
        [Serializable]
        public class EmblemProperties : ResponseObject
        {
            /// <summary>
            /// Background ID
            /// </summary>
            [JsonProperty("background_id")]
            public int BackgroundId { get; set; }
            /// <summary>
            /// Foreground id
            /// </summary>
            [JsonProperty("foreground_id")]
            public int ForegroundId { get; set; }
            /// <summary>
            /// flags
            /// </summary>
            [JsonProperty("flags")]
            public string[] Flags { get; set; }
            /// <summary>
            /// Background color id.
            /// </summary>
            [JsonProperty("background_color_id")]
            public int BackgroundColorId { get; set; }
            /// <summary>
            /// Foreground primary color id.
            /// </summary>
            [JsonProperty("foreground_primary_color_id")]
            public int ForegroundPrimaryColorId { get; set; }
            /// <summary>
            /// Foreground secondary color id.
            /// </summary>
            [JsonProperty("foreground_secondary_color_id")]
            public int ForegroundSecondaryColorId { get; set; }

        }
    }
}
