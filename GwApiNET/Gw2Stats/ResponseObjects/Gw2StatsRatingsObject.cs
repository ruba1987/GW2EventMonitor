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
    [JsonConverter(typeof(Gw2StatsRatingsObjectConverter))]
    public class Gw2StatsRatingsObject : ResponseObject
    {
        [JsonProperty("retrieve_time")]
        public DateTime RetrieveTime { get; set; }
        [JsonProperty("results")]
        public int Results { get; set; }

        //private IDictionary<string, Gw2StatsRatingsRegion> _region;
        //public IDictionary<string, Gw2StatsRatingsRegion> Regions { get { return _region ?? (_region = new Dictionary<string, Gw2StatsRatingsRegion>()); } set { _region = value; } }
        private IDictionary<string, IDictionary<int, Gw2StatsRatingsEntry>> _ratings;
        public IDictionary<string, IDictionary<int, Gw2StatsRatingsEntry>> Ratings { get { return _ratings ?? (_ratings = new Dictionary<string, IDictionary<int, Gw2StatsRatingsEntry>>()); } set { _ratings = value; } }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsRatingsObject()
        {
        }
    }

    [Serializable]
    public class Gw2StatsRatingsRegion
    {
        private IDictionary<int, Gw2StatsRatingsEntry> _ratings;
        [JsonProperty("ratings")]
        public IDictionary<int, Gw2StatsRatingsEntry> Ratings { get { return _ratings ?? (_ratings = new Dictionary<int, Gw2StatsRatingsEntry>()); } set { _ratings = value; } }
    }

    [Serializable]
    public class Gw2StatsRatingsEntry
    {
        [JsonProperty("world_id")]
        public int WorldId { get; set; }
        [JsonProperty("world_name")]
        public string WorldName { get; set; }
        public Gw2StatsRatingsData Data { get; set; }
    }

    [Serializable]
    public class Gw2StatsRatingsData
    {
        [JsonProperty("start_rank")]
        public int StartRank { get; set; }
        [JsonProperty("current_rank")]
        public int CurrentRank { get; set; }
        [JsonProperty("start_rating")]
        public double StartRating { get; set; }
        [JsonProperty("start_deviation")]
        public double StartDeviation { get; set; }
        [JsonProperty("current_deviation")]
        public double CurrentDeviation { get; set; }
        [JsonProperty("volatility")]
        public double Volatility { get; set; }
        [JsonProperty("current_rating")]
        public double CurrentRating { get; set; }
        [JsonProperty("evolution")]
        public double Evolution { get; set; }
    }

    [Serializable]
    public class Gw2StatsRatingsObjectConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            JObject jobject = JObject.Load(reader);


            Gw2StatsRatingsObject obj = new Gw2StatsRatingsObject();

            foreach (var kvp in jobject)
            {
                if (kvp.Key == "retrieve_time") obj.RetrieveTime = ((DateTime) kvp.Value).ToUniversalTime();
                else if (kvp.Key == "results") obj.Results = (int) kvp.Value;
                else
                {
                    var region = kvp.Value.ToObject<Gw2StatsRatingsRegion>(serializer);
                    obj.Ratings.Add(kvp.Key, region.Ratings);
                }
            }
            return obj;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ApiStatus).IsAssignableFrom(objectType);
        }
    }

}
