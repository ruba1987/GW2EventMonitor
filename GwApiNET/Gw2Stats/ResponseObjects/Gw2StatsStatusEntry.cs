using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GwApiNET.Gw2Stats.ResponseObjects
{

    [Serializable]
    public class Gw2StatsStatusEntry : ResponseObject
    {

        public string Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("status")]
        public ApiStatus Status { get; set; }
        public string StatusDescription { get { return Gw2StatsApi.StatusCodes[Status]; } }
        [JsonProperty("ping")]
        public int Ping { get; set; }
        [JsonProperty("retrieve")]
        public int Retrieve { get; set; }
        [JsonProperty("records")]
        public int Records { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsStatusEntry()
        {
        }
    }

    [Serializable]
    public class JsonApiStatusConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JProperty property = JProperty.Load(reader);
            string value = property["status"].ToString();
            ApiStatus status = (ApiStatus)Enum.Parse(typeof (ApiStatus), value.Replace("_", " "));
            return status;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ApiStatus).IsAssignableFrom(objectType);
        }
    }

    [Serializable]
    public enum ApiStatus
    {
        /// <summary>
        /// Status
        /// "No error was received : Completed successfully"
        /// </summary>
        [Description("No error was received : Completed successfully")]
        OK,
        /// <summary>
        /// Status
        /// "The host was unreachable : host may be down"
        /// </summary>
        [Description("The host was unreachable : host may be down")]
        UNREACHABLE,
        /// <summary>
        /// Status
        /// "Host was reachable but API returned error : API may be down"
        /// </summary>
        [Description("Host was reachable but API returned error : API may be down")]
        DOWN,
        /// <summary>
        /// Status
        /// "Only partial data was received"
        /// </summary>
        [Description("Only partial data was received")]
        PARTIAL,
        /// <summary>
        /// Status
        /// "Ping to the API host has increased 100ms from last update"
        /// </summary>
        [Description("Ping to the API host has increased 100ms from last update")]
        INCREASING,
        /// <summary>
        /// Status
        /// "Ping to the API host has reached a minimum of 750ms"
        /// </summary>
        [Description("Ping to the API host has reached a minimum of 750ms")]
        HIGH_PING,
        /// <summary>
        /// Status
        /// "API host and API are up : Data retrieval is over 3 seconds"
        /// </summary>
        [Description("API host and API are up : Data retrieval is over 3 seconds")]
        SLOW_RETRIEVE,
    }
}
