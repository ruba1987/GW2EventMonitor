using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GwApiNET.CacheStrategy;
using GwApiNET.ResponseObjects;

namespace GwApiNET
{

    public static class Constants
    {
        static Constants()
        {

        }

        public static Dictionary<string, ICacheStrategy> CacheStrategies = new Dictionary<string, ICacheStrategy>()
            {
                {typeof (ColorEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (ContinentEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (EventEntry).FullName, new AgeCacheStrategy(TimeSpan.FromMinutes(30))},
                {typeof (EventNameEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (GuildDetailsEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (IdList).FullName, new BuildVersionCacheStrategy()},
                {typeof (ItemDetailsEntry).FullName, NullCacheStrategy.NullStrategy},
                {typeof (MapEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (MapFloorEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (MapNameEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (MatchDetailsEntry).FullName, new AgeCacheStrategy(TimeSpan.FromSeconds(30))},
                {typeof (MatchEntry).FullName, new AgeCacheStrategy(TimeSpan.FromSeconds(30))},
                {typeof (ObjectiveNameEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (RecipeDetailsEntry).FullName, NullCacheStrategy.NullStrategy},
                {typeof (WorldNameEntry).FullName, new BuildVersionCacheStrategy()},
                {typeof (FileEntry).FullName, new AgeCacheStrategy(TimeSpan.FromMinutes(30))},
                {typeof (Gw2PositionReader.Player.Identity).FullName, new AgeCacheStrategy(TimeSpan.FromSeconds(3))},
            };

        public static ICacheStrategy GetCacheStrategy(Type type)
        {
            if(type.IsSubclassOf(typeof(ResponseObject)))
            {
                return CacheStrategies.ContainsKey(type.FullName)
                           ? CacheStrategies[type.FullName]
                           : NullCacheStrategy.NullStrategy;
            }
            return NullCacheStrategy.NullStrategy;
        }

        public static ICacheStrategy GetCacheStrategy<T>() where T : ResponseObject
        {
            return GetCacheStrategy(typeof (T));
        }

        public static ICacheStrategy GetCacheStrategy(ResponseObject response)
        {
            return GetCacheStrategy(response.GetType());
        }

        public static void SetCacheStrategy(ResponseObject response, ICacheStrategy strategy)
        {
            SetCacheStrategy(response.GetType(), strategy);
        }

        public static void SetCacheStrategy<T>(ICacheStrategy strategy)
        {
            SetCacheStrategy(typeof(T), strategy);
        }
        public static void SetCacheStrategy(Type type, ICacheStrategy strategy)
        {
            CacheStrategies[type.FullName] = strategy;
        }

        /// <summary>
        /// File path to Cached Responses
        /// </summary>
        public static string CacheFile = "ResponseCache.bin";
        
        [XmlIgnore]
        public static SerializableDictionary<string, TimeSpan> ExpirationTime = new SerializableDictionary<string, TimeSpan>()
            {
                {typeof (ColorEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (ContinentEntry).FullName, TimeSpan.FromDays(30)},
                {typeof (EventEntry).FullName, TimeSpan.FromMinutes(2)},
                {typeof (EventNameEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (GuildDetailsEntry).FullName, TimeSpan.FromDays(30)},
                {typeof (IdList).FullName, TimeSpan.FromDays(7)},
                {typeof (ItemDetailsEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (MapEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (MapFloorEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (MapNameEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (MatchDetailsEntry).FullName, TimeSpan.FromSeconds(30)},
                {typeof (MatchEntry).FullName, TimeSpan.FromSeconds(30)},
                {typeof (ObjectiveNameEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (RecipeDetailsEntry).FullName, TimeSpan.FromDays(7)},
                {typeof (WorldNameEntry).FullName, TimeSpan.FromDays(30)},

            };

        /// <summary>
        /// Log Filename.
        /// </summary>
        public static string LoggerFileName = "GwApiNETLog_.txt";

        /// <summary>
        /// General Logger.
        /// </summary>
        public static IList<string> LoggerNames = new List<string>
            {
                "GwApiNETLogger"
            };

        #region Library Settings
        /// <summary>
        /// Current default Language,  this added to api calls automatically.
        /// </summary>
        public static Language CurrentLanguage = Language.English;
        #endregion Library Settings

        #region Persistance

        // returns any ICacheStrategy type that is currently being used
        private static IEnumerable<Type> _getCacheStrategyTypes()
        {
            List<Type> types = new List<Type>();
            foreach (var pair in CacheStrategies)
            {
                types.Add(pair.Value.GetType());
            }
            return types;
        }

        public static void LoadConstants(Stream stream)
        {
            Type type = typeof(Constants);
            var members = type.GetMembers();
            using(TextReader reader = new StreamReader(stream))
            {
                string xml = reader.ReadToEnd();
                reader.Close();
                try
                {
                    // DataContract Serializer needs to known some special types
                    var knownTypes = new List<Type>
                        {
                            typeof (Dictionary<string, ICacheStrategy>),
                            typeof(NullCacheStrategy),
                            typeof (Language),
                            typeof (string[]),
                        };
                    // we use this to ensure custom types are added when they are used
                    knownTypes.AddRange(_getCacheStrategyTypes());
                    var memberDeserialized = XmlUtilities.DataContractDeserializer<Dictionary<string, object>>(xml, knownTypes);
                    foreach (var pair in memberDeserialized)
                    {
                        string name = pair.Key;
                        var member = members.Single(f => f.Name == name);
                        if (member != null)
                        {
                            try
                            {
                                if (member is FieldInfo)
                                {
                                    FieldInfo field = member as FieldInfo;
                                    field.SetValue(null, pair.Value);
                                }
                                else if (member is PropertyInfo)
                                {
                                    PropertyInfo property = member as PropertyInfo;
                                    property.SetValue(null, pair.Value);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(string.Format("Failed for member {0}", member.Name));
                                Debug.WriteLine(e);
                                GwApi.Logger.Error(e);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    GwApi.Logger.Error(e);
                }
            }
        }
        /// <summary>
        /// Load the constants from a file.
        /// Only constants listed in file are loaded, otherwise defaults are used.
        /// <remarks>Optional</remarks>
        /// </summary>
        /// <param name="file">file path</param>
        public static void LoadConstants(string file = "Constants.xml")
        {
            if (File.Exists(file)) LoadConstants(File.OpenRead(file));
        }
        public static void SaveConstants(Stream stream)
        {
            Type type = typeof(Constants);
            var members = type.GetMembers();
            try
            {
                var memberDictionary = new Dictionary<string, object>(members.Length);
                foreach (var member in members)
                {
                    try
                    {
                        if (member is FieldInfo)
                        {
                            FieldInfo field = member as FieldInfo;
                            if(!field.GetCustomAttributes<XmlIgnoreAttribute>().Any())
                                memberDictionary.Add(field.Name, field.GetValue(null));
                        }
                        else if (member is PropertyInfo)
                        {
                            PropertyInfo property = member as PropertyInfo;
                            if (!property.GetCustomAttributes<XmlIgnoreAttribute>().Any())
                                memberDictionary.Add(property.Name, property.GetValue(null));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        GwApi.Logger.Error(e);
                    }
                }
                var knownTypes = new List<Type>
                    {
                        typeof(Dictionary<string, ICacheStrategy>),
                        typeof(List<string>),
                        typeof(Language),
                        //typeof(string[]),
                    };
                string xml = XmlUtilities.DataContractSerializer(memberDictionary, knownTypes);
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.Write(xml);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                GwApi.Logger.Error(e);
            }
        }
        /// <summary>
        /// Save the constants form a file
        /// <remarks>Optional</remarks>
        /// </summary>
        /// <param name="file">file path</param>
        public static void SaveConstants(string file = "Constants.xml")
        {
            using (Stream stream = File.Open(file, FileMode.Create))
            {
                SaveConstants(stream);
            }
        }
        #endregion Persistance

        #region GW Api Urls
        /// <summary>
        /// Base url for all API calls
        /// </summary>
        public static string ApiBaseUrl = "https://api.guildwars2.com/v1";
        /// <summary>
        /// events.json api url
        /// </summary>
        public static string events = "events.json";
        /// <summary>
        /// event_names.json api url
        /// </summary>
        public static string event_names = "event_names.json";
        /// <summary>
        /// map_names.json api url
        /// </summary>
        public static string map_names = "map_names.json";
        /// <summary>
        /// world_names.json api url
        /// </summary>
        public static string world_names = "world_names.json";
        /// <summary>
        /// event_details.json api url
        /// </summary>
        public static string event_details = "event_details.json";
        /// <summary>
        /// wvw/matches.json api url
        /// </summary>
        public static string wvw_matches = "wvw/matches.json";
        /// <summary>
        /// wvw/match_details.json api url
        /// </summary>
        public static string wvw_match_details = "wvw/match_details.json";
        /// <summary>
        /// wvw/objective_names.json api url
        /// </summary>
        public static string wvw_objective_names = "wvw/objective_names.json";
        /// <summary>
        /// items.json api url
        /// </summary>
        public static string items = "items.json";
        /// <summary>
        /// item_details.json api url
        /// </summary>
        public static string item_details = "item_details.json";
        /// <summary>
        /// recipes.json api url
        /// </summary>
        public static string recipes = "recipes.json";
        /// <summary>
        /// recipe_details.json api url
        /// </summary>
        public static string recipe_details = "recipe_details.json";
        /// <summary>
        /// guild_details.json api url
        /// </summary>
        public static string guild_details = "guild_details.json";
        /// <summary>
        /// continents.json api url
        /// </summary>
        public static string continents = "continents.json";
        /// <summary>
        /// maps.json api url
        /// </summary>
        public static string maps = "maps.json";
        /// <summary>
        /// map_floor.json api url
        /// </summary>
        public static string map_floor = "map_floor.json";
        /// <summary>
        /// build.json api url
        /// </summary>
        public static string build = "build.json";
        /// <summary>
        /// colors.json api url
        /// </summary>
        public static string colors = "colors.json";
        /// <summary>
        /// files.json api uri
        /// </summary>
        public static string files = "files.json";
        #endregion GW Api Urls

        #region Gw2Stats Constants
        #region Gw2Stats Api Urls

        public static string gw2_ApiBaseUrl = "http://gw2stats.net/api";
        public static string gw2_status = "status.json";
        public static string gw2_status_codes = "status_codes.json";
        public static string gw2_matches = "matches.json";
        public static string gw2_objectives = "objectives.json";
        public static string gw2_ratings = "ratings.json";

        #endregion Gw2Stats Api Urls

        #endregion Gw2Stats Constants

        #region GW Maps Constants

        public static int TileSize = 256;
        public static int PixelToWorldPosRatio = 24;
        public static double WorldPosToPixelRatio = 1.0 / PixelToWorldPosRatio;

        public static IList<string> ContinentErrorTileUrl = new List<string>
            {
                "",
                ""
            };

        public static string MapTileUrlFormat = "https://tiles.guildwars2.com/{0}/{1}/{2}/{3}/{4}.jpg";

        #endregion GW Maps Constants

        #region Render Service Constants

        public static string RenderServiceBaseUri = "https://render.guildwars2.com/file";

        public static string RenderServiceUriFormat = "{0}/{1}.{2}";

        #endregion Render Service Constants
    }
}
