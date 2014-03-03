using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using GwApiNET.ResponseObjects.Parsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GwApiNET.Gw2Stats.ResponseObjects
{
    [Serializable]
    internal class _ObjectivesByWorld : Gw2StatsMap
    {
        [JsonProperty("retrieve_time")]
        public DateTime RetriveTime { get; set; }
        [JsonProperty("type")]
        public Gw2StatsObjectives.ObjectiveType Type { get; set; }
        [JsonProperty("match_id")]
        public string MatchId { get; set; }

    }

    [Serializable]
    internal class _ObjectivesByMatch : Gw2StatsMatch
    {
        [JsonProperty("retrieve_time")]
        public DateTime RetriveTime { get; set; }
        [JsonProperty("type")]
        public Gw2StatsObjectives.ObjectiveType Type { get; set; }
    }
    [Serializable]
    [JsonConverter(typeof(Gw2StatsObjectivesConverter))]
    public class Gw2StatsObjectives : ResponseObject
    {
        #region Constructors

        internal Gw2StatsObjectives(_ObjectivesByMatch obm)
        {
            RetriveTime = obm.RetriveTime;
            Type = obm.Type;
            Matches = new Dictionary<string, Gw2StatsMatch>
                {
                    {
                        obm.MatchId,
                        new Gw2StatsMatch
                            {
                                Maps = obm._maps.ToDictionary(m => m.MapId),
                                MatchId = obm.MatchId,
                            }
                    }
                };
        }

        internal Gw2StatsObjectives(_ObjectivesByWorld obw)
        {
            RetriveTime = obw.RetriveTime;
            Type = obw.Type;
            Matches = new Dictionary<string, Gw2StatsMatch>()
                {
                    {
                        obw.MatchId, new Gw2StatsMatch()
                            {
                                MatchId = obw.MatchId,
                                Maps = new Dictionary<int, Gw2StatsMap>
                                    {
                                        {
                                            obw.MapId, new Gw2StatsMap
                                                {
                                                    MapId = obw.MapId,
                                                    MapOwnerId = obw.MapOwnerId,
                                                    MapOwnerName = obw.MapOwnerName,
                                                    Name = obw.Name,
                                                    Objectives = obw.Objectives,
                                                }
                                        }
                                    }
                            }
                    }
                };
        }

        #endregion Constructors

        [JsonProperty("retrieve_time")]
        public DateTime RetriveTime { get; set; }
        [JsonProperty("type")]
        public ObjectiveType Type { get; set; }

        private Dictionary<string, Gw2StatsMatch> _matches;
        public Dictionary<string, Gw2StatsMatch> Matches { get { return _matches ?? (_matches = new Dictionary<string, Gw2StatsMatch>()); } set { _matches = value; } }
        //public List<Gw2StatsMatch> Matches1 { get; set; }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsObjectives()
        {
        }

        public enum ObjectiveType
        {
            All,
            Match,
            World,
        }
    }

    [Serializable]
    public class Gw2StatsMatch
    {
        [JsonProperty("match_id")]
        public string MatchId { get; set; }
        [JsonProperty("maps")]
        internal IList<Gw2StatsMap> _maps { get; set; }

        private IDictionary<int, Gw2StatsMap> _mapsDict;
        public IDictionary<int, Gw2StatsMap> Maps { get { return _mapsDict ?? (_mapsDict = new Dictionary<int, Gw2StatsMap>()); } set { _mapsDict = value; } }
    }

    [Serializable]
    public class Gw2StatsMap
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("map_id")]
        public int MapId { get; set; }
        [JsonProperty("map_owner_id")]
        public int MapOwnerId { get; set; }
        [JsonProperty("map_owner_name")]
        public string MapOwnerName { get; set; }

        private EntryDictionary<int, Gw2StatsMapObjective> _objectives;
        [JsonProperty("objectives")]
        public EntryDictionary<int, Gw2StatsMapObjective> Objectives { get { return _objectives ?? (_objectives = new EntryDictionary<int, Gw2StatsMapObjective>()); } set { _objectives = value; } }

    }

    [Serializable]
    public class Gw2StatsMapObjective
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cardinal")]
        public string Cardinal { get; set; }
        [JsonProperty("points")]
        public int Points { get; set; }
        [JsonProperty("capture_time")]
        public DateTime CaptureTime { get; set; }
        [JsonProperty("time_held")]
        public TimeSpan TimeHeld { get; set; }

        private Gw2StatsOwner _currentOwner;
        private Gw2StatsOwner _previousOwner;
        private Gw2StatsCurrentGuild _currentGuild;
        [JsonProperty("current_owner")]
        public Gw2StatsOwner CurrentOwner { get { return _currentOwner ?? (_currentOwner = new Gw2StatsOwner()); } set { _currentOwner = value; } }
        [JsonProperty("previous_owner")]
        public Gw2StatsOwner PreviousOwner { get { return _previousOwner ?? (_previousOwner = new Gw2StatsOwner()); } set { _previousOwner = value; } }
        [JsonProperty("current_guild")]
        public Gw2StatsCurrentGuild CurrentGuild { get { return _currentGuild ?? (_currentGuild = new Gw2StatsCurrentGuild()); } set { _currentGuild = value; } }

        [Serializable]
        public class Gw2StatsOwner
        {
            [JsonProperty("world_id")]
            public int WorldId { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("color")]
            public OwnerColor Color { get; set; }
        }
    }

    [Serializable]
    public class Gw2StatsCurrentGuild
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("img_url")]
        public string ImageUrl { get; set; }
    }

    [Serializable]
    public class Gw2StatsObjectivesConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            JObject jobject = JObject.Load(reader);

            Gw2StatsObjectives obj = new Gw2StatsObjectives();

            foreach (var kvp in jobject)
            {
                if (kvp.Key == "retrieve_time")
                {
                    obj.RetriveTime = ((DateTime)kvp.Value).ToUniversalTime();
                }
                else if (kvp.Key == "type") obj.Type = kvp.Value.ToObject<Gw2StatsObjectives.ObjectiveType>(serializer);
                else
                {
                    Gw2StatsMatch match = kvp.Value.ToObject<Gw2StatsMatch>();
                    var matchreader = kvp.Value.CreateReader();
                    JObject jmatch = JObject.Load(matchreader);
                    foreach (var kvp2 in jmatch)
                    {
                        if (kvp2.Key == "match_id") match.MatchId = kvp2.Value.ToString();
                        else
                        {
                            var map = kvp2.Value.ToObject<Gw2StatsMap>(serializer);
                            match.Maps.Add(map.MapId, map);
                        }
                    }
                    var temp = kvp.Value;

                    obj.Matches.Add(kvp.Key, match);
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
