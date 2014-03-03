using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.Logging;
using GwApiNET.ResponseObjects;
using GwApiNET.ResponseObjects.Parsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Slf;
using Slf.BitFactoryFacade;
using Slf.Factories;
using Slf.Resolvers;

namespace GwApiNET
{

    public static class GwApi
    {
        private static string LoggerName
        {
            get { return Constants.LoggerNames[0]; }
        }

        private static ILogger _logger = NullLogger.Instance;
        internal static ILogger Logger { get { return _logger; } }
        public static INetworkHandler Network;
        public static INetworkHandler RenderServiceNetwork;
        internal static RestClient Client;
        private static int _build = -1;
        /// <summary>
        /// GW2 Build Version.
        /// <remarks>This value is updated each time the library is initialized.</remarks>
        /// </summary>
        public static int Build
        {
            get { return _build < 0 ? (_build = GetBuildNumber()) : _build; }
            private set { _build = value; }
        }

        #region Load/Unload

        static GwApi()
        {
            Client = new RestClient(Constants.ApiBaseUrl);
            Network = new NetworkHandler(Language.English, Constants.ApiBaseUrl);
            //RenderServiceNetwork = new RenderServiceNetworkHandler(Language.English);
            RenderServiceNetwork = new NetworkHandler(Language.English, Constants.RenderServiceBaseUri);
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            try
            {
                _setupLogger();
            }
            catch (Exception e)
            {
                string msg = string.Format("{0} - Error creating logger\n{1}\n", DateTime.Now,e);
                File.AppendAllText("Logging_Error.txt", msg);
            }
        }
        
        private static void _setupLogger()
        {
            // rolling file logger
            ILogger logger = BitFactoryLogger.CreateRollingFileLogger(Constants.LoggerFileName, "yyyyMMdd", LoggerName, false);
            _logger = logger;
            GwLogManager.RegisterLogger(LoggerName, logger);
        }

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {
                ResponseCache.Cache.Save();
            }
            catch (Exception exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}-------------------", DateTime.Now));
                sb.AppendLine(exception.ToString());
                sb.AppendLine("---");
                File.AppendAllText("Exception.txt", sb.ToString());
            }
        }

        #endregion Load/Unload

        #region Sync Api Calls
        #region Dynamic Events API – BETA

        /// <summary>
        /// Obtains an event mapping between world_id, event_id, and map_id
        /// <remarks>All parameters are optional.  Default values will ommit all parameters and retrieve a complete events list</remarks>
        /// </summary>
        /// <param name="world_id">(Optional) provide a value > 0 to include this as a parameter</param>
        /// <param name="map_id">(Optional) provide a value > 0 to include this as a parameter</param>
        /// <param name="event_id">(Optional) provide a non-null value to include this as a parameter</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>event mapping between world_id, event_id, and map_id</returns>
        [Tested("GetEventsTest")]
        public static EntryCollection<EventEntry> GetEvents(int world_id = -1, int map_id = -1, Guid? event_id = null,
                                                            bool ignoreCache = false)
        {
            Logger.Info("Request for Event Entry");
            var parser = new EventEntryParser();
            ApiRequest request = new ApiRequest(Constants.events);
            if (world_id > 0) request.AddParameter("world_id", world_id);
            if (map_id > 0) request.AddParameter("map_id", map_id);
            if (event_id != null) request.AddParameter("event_id", event_id.ToString());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Event Names
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetEventNamesTest")]
        public static EntryDictionary<Guid, EventNameEntry> GetEventNames(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Event Names");
            var parser = new EventNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.event_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve World Name Information
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetWorldNamesTest")]
        public static EntryDictionary<int, WorldNameEntry> GetWorldNames(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for World Names");
            var parser = new WorldNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.world_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Map Name information
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetMapNamesTest")]
        public static EntryDictionary<int, MapNameEntry> GetMapNames(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Map Names");
            var parser = new MapNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.map_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Event Details
        /// </summary>
        /// <param name="event_id">(Optional) id of event to Retrieve. Use Guid.Empty to omit event_id and retrieve details for all events</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Collection of a single <seealso cref="EventDetailsEntry"/> when event_id is null, otherwise a <seealso cref="EntryCollection{EventDetailsEntry}"/></returns>
        [Tested("GetEventDetailsTest")]
        public static EntryDictionary<Guid, EventDetailsEntry> GetEventDetails(Guid event_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Event Details for ID: {0}", event_id.ToString());
            var parser = new EventDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.event_details);
            request.AddParameter("lang", lang.Code());
            if (event_id != Guid.Empty)
                request.AddParameter("event_id", event_id);
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Event Details
        /// </summary>
        /// <param name="event_id">(Optional) id of event to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Collection of a single <seealso cref="EventDetailsEntry"/> when event_id is null, otherwise a <seealso cref="EntryCollection{EventDetailsEntry}"/></returns>
        [Tested("GetEventDetailsTest")]
        public static EntryDictionary<Guid, EventDetailsEntry> GetEventDetails(string event_id = null, bool ignoreCache = false, Language lang = Language.English)
        {
            if (event_id == null)
                return GetEventDetails(Guid.Empty, ignoreCache);
            return GetEventDetails(new Guid(event_id), ignoreCache, lang);
        }

        #endregion Dynamic Events API – BETA

        #region Item and Recipe Database API – BETA
        /// <summary>
        /// Retrieve List of all Item IDs discovered by players
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Item IDs discovered by players</returns>
        [Tested("GetItemIdsTest")]
        public static IdList GetItemIds(bool ignoreCache = false)
        {
            Logger.Info("Request for Item Id List");
            var parser = new ItemIdListParser();
            ApiRequest request = new ApiRequest(Constants.items);
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve List of all Recipe IDs discovered by players
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Recipe IDs discovered by players</returns>
        [Tested("GetRecipeIdsTest")]
        public static IdList GetRecipeIds(bool ignoreCache = false)
        {
            Logger.Info("Request for Recipe Id List");
            var parser = new RecipeIdListParser();
            ApiRequest request = new ApiRequest(Constants.recipes);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Retrieves the details for a given item.
        /// </summary>
        /// <param name="item_id">(Required) - Item id</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>details for the given item id</returns>
        [Tested("GetItemDetailsTest")]
        public static ItemDetailsEntry GetItemDetails(int item_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Item: {0}", item_id);
            var parser = new ItemDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.item_details);
            request.AddParameter("lang", lang.Code());
            request.AddParameter("item_id", item_id);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Retrieve Recipe information
        /// </summary>
        /// <param name="recipe_id">(Required) id of recipe to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Recipe information</returns>
        [Tested("GetRecipeDetailsTest")]
        public static RecipeDetailsEntry GetRecipeDetails(int recipe_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Recipe: {0}", recipe_id);
            var parser = new RecipeDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.recipe_details);
            request.AddParameter("lang", lang.Code());
            request.AddParameter("recipe_id", recipe_id);
            return HandleRequest(request, parser, ignoreCache);
        }

        #endregion Item and Recipe Database API – BETA

        #region Guild API – BETA

        /// <summary>
        /// Retrieve Guild Information for a given Guild Name
        /// </summary>
        /// <param name="guild_name">(Required) Name of guild to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Guild information</returns>
        [Tested("GetGuildDetailsByNameTest")]
        public static GuildDetailsEntry GetGuildDetailsByName(string guild_name, bool ignoreCache = false)
        {
            Logger.Info("Request for {0} - {1}", typeof(GuildDetailsEntry).Name, guild_name);
            var parser = new GuildDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.guild_details);
            request.AddParameter("guild_name", guild_name);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Retrieve Guild Information for a given Guild ID
        /// </summary>
        /// <param name="guild_id">(Required) id of guild to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Guild information</returns>
        [Tested("GetGuildDetailsByIdTest")]
        public static GuildDetailsEntry GetGuildDetailsById(string guild_id, bool ignoreCache = false)
        {
            return GetGuildDetailsById(new Guid(guild_id), ignoreCache);
        }

        /// <summary>
        /// Retrieve Guild Information for a given Guild ID
        /// </summary>
        /// <param name="guild_id">(Required) id of guild to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Guild information</returns>
        [Tested("GetGuildDetailsByIdTest")]
        public static GuildDetailsEntry GetGuildDetailsById(Guid guild_id, bool ignoreCache = false)
        {
            Logger.Info("Request for {0} - {1}", typeof(GuildDetailsEntry).Name, guild_id.ToString());
            var parser = new GuildDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.guild_details);
            request.AddParameter("guild_id", guild_id);
            return HandleRequest(request, parser, ignoreCache);
        }

        #endregion Guild API – BETA

        #region WvW API – BETA

        /// <summary>
        /// Retrieve a list of Matches
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>List of <seealso cref="MatchEntry"/></returns>
        [Tested("GetMatchesTest")]
        public static EntryDictionary<string, MatchEntry> GetMatches(bool ignoreCache = false)
        {
            Logger.Info("Request for Matches");
            var parser = new MatchEntryParser();
            ApiRequest request = new ApiRequest(Constants.wvw_matches);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Retrieve Match Details
        /// </summary>
        /// <param name="match_id">(Required) id of the match to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="MatchDetailsEntry"/> containing information for a given match</returns>
        [Tested("GetMatchDetailsTest")]
        public static MatchDetailsEntry GetMatchDetails(string match_id, bool ignoreCache = false)
        {
            Logger.Info("Request for Match ID: {0}", match_id);
            var parser = new MatchDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.wvw_match_details);
            request.AddParameter("match_id", match_id);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Get WvW ObjectiveNames
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>ObjectiveNames</returns>
        [Tested("GetObjectiveNamesTest")]
        public static EntryDictionary<int, ObjectiveNameEntry> GetObjectiveNames(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Objective Names");
            var parser = new ObjectiveNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.wvw_objective_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }

        #endregion WvW API – BETA

        #region Map API – BETA

        /// <summary>
        /// Retrieve a list of contintents.
        /// <remarks>Indexed by continent ID</remarks>
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetContinentsTest")]
        public static EntryDictionary<int, ContinentEntry> GetContinents(bool ignoreCache = false)
        {
            Logger.Info("Request for Continents");
            var parser = new ContinentEntryParser();
            ApiRequest request = new ApiRequest(Constants.continents);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Retrieve Map Entries
        /// <remarks>Indexed by Map ID</remarks>
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="EntryCollection{MapEntry}"/></returns>
        [Tested("GetMapTest")]
        public static EntryDictionary<int, MapEntry> GetMap(bool ignoreCache = false, Language lang = Language.English)
        {
            return GetMap(-1, ignoreCache, lang);
        }

        /// <summary>
        /// Retrieve Map Entries
        /// <remarks>Indexed by Map ID</remarks>
        /// </summary>
        /// <param name="map_id">(Optional) Map id of the map entry to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="EntryCollection{MapEntry}"/></returns>
        [Tested("GetMapTest")]
        public static EntryDictionary<int, MapEntry> GetMap(int map_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Map Entry ID: {0}", map_id);
            var parser = new MapEntryParser();
            ApiRequest request = new ApiRequest(Constants.maps);
            request.AddParameter("lang", lang.Code());
            if (map_id > 0) request.AddParameter("map_id", map_id);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// Map Floor information. Useful when using the world map tile service to display maps.
        /// </summary>
        /// <param name="continent_id">(Required) Continent to obtain info for</param>
        /// <param name="floor">(Required) floor of continent, check <seealso cref="ContinentEntry"/> for a list of floors</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="MapFloorEntry"/></returns>
        [Tested("GetMapFloorTest")]
        public static MapFloorEntry GetMapFloor(int continent_id, int floor, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Map floor {0} on Continent {1}", floor, continent_id);
            var parser = new MapFloorEntryParser();
            ApiRequest request = new ApiRequest(Constants.map_floor);
            request.AddParameter("lang", lang.Code());
            request.AddParameter("continent_id", continent_id);
            request.AddParameter("floor", floor);
            return HandleRequest(request, parser, ignoreCache);
        }

        #endregion Map API – BETA

        #region Miscellaneous APIs – BETA

        /// <summary>
        /// Get Colors used for Guild Emblems
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Color Details</returns>
        [Tested("GetColorsTest")]
        public static EntryDictionary<int, ColorEntry> GetColors(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Colors");
            var parser = new ColorEntryParser();
            ApiRequest request = new ApiRequest(Constants.colors);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }


        /// <summary>
        /// Retrieve the current game build version
        /// </summary>
        /// <returns>game build version</returns>
        [Tested("GetBuildTest")]
        public static int GetBuildNumber()
        {
            ApiRequest request = new ApiRequest(Constants.build);
            var apiResponse = Network.GetResponse(request) as IRestResponse;
            if (apiResponse == null) return -1;
            var json = JsonConvert.DeserializeObject(apiResponse.Content) as JObject;
            if (json != null)
                _build = (int) json["build_id"];
            return _build;
        }

        /// <summary>
        /// Retrives a list of files that can be used with the Render Services api.
        /// Provides icons and images for maps, items, etc..
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetFilesTest")]
        public static EntryDictionary<string, FileEntry> GetFiles(bool ignoreCache = false)
        {
            Logger.Info("Request for File Entries");
            var parser = new FileEntryParser();
            ApiRequest request = new ApiRequest(Constants.files);
            return HandleRequest(request, parser, ignoreCache);
        }

        /// <summary>
        /// The render service provides access to in-game assets like item icons.
        /// </summary>
        /// <param name="signature">(Required) File signature (Check <seealso cref="ItemDetailsEntry"/> or <seealso cref="FileEntry"/> for valid signatures)</param>
        /// <param name="file_id">(Required) File ID (Check <seealso cref="ItemDetailsEntry"/> or <seealso cref="FileEntry"/> for valid file id's)</param>
        /// <param name="format">(Required) Image format, Currently .png or .jpg</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetRenderServiceAssetEntryTest")]
        public static RenderServiceAssetEntry GetRenderServiceAssetEntry(string signature, int file_id, string format, bool ignoreCache = false)
        {
            return GetRenderServiceAssetEntry(new FileEntry() {Signature = signature, FileID = file_id}, format, ignoreCache);
        }

        /// <summary>
        /// The render service provides access to in-game assets like item icons.
        /// </summary>
        /// <param name="file">(Required) File entry contining a signature and id</param>
        /// <param name="format">(Required) Image format, Currently .png or .jpg</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetRenderServiceAssetEntryTest")]
        public static RenderServiceAssetEntry GetRenderServiceAssetEntry(FileEntry file, string format, bool ignoreCache = false)
        {
            Logger.Info("Request for {0} File:{1} Sig:{2}", format, file.FileID, file.Signature);
            var parser = new RenderServiceAssetParser();
            IApiRequest request = new ApiRequest(
                string.Format(Constants.RenderServiceUriFormat, file.Signature, file.FileID, format));
            RenderServiceAssetEntry entry = HandleRequest(request, parser, RenderServiceNetwork, ignoreCache);
            entry.File = file;
            return entry;
        }

        #endregion Miscellaneous APIs – BETA

        #endregion Sync Api Calls

        #region Async Api Calls
        #region Dynamic Events API - BETA

        [Tested("GetEventsTest")]
        public static async Task<EntryCollection<EventEntry>> GetEventsAsync(int world_id = -1, int map_id = -1,
                                                                             Guid? event_id = null,
                                                                             bool ignoreCache = false)
        {
            Logger.Info("Request for Event Entry");
            var parser = new EventEntryParser();
            ApiRequest request = new ApiRequest(Constants.events);
            if (world_id > 0) request.AddParameter("world_id", world_id);
            if (map_id > 0) request.AddParameter("map_id", map_id);
            if (event_id != null) request.AddParameter("event_id", event_id.ToString());
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve Event Names
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetEventNamesTest")]
        public async static Task<EntryDictionary<Guid, EventNameEntry>> GetEventNamesAsync(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Event Names");
            var parser = new EventNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.event_names);
            request.AddParameter("lang", lang.Code());
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        /// <summary>
        /// Retrieve World Name Information
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetWorldNamesTest")]
        public async static Task<EntryDictionary<int, WorldNameEntry>> GetWorldNamesAsync(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for World Names");
            var parser = new WorldNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.world_names);
            request.AddParameter("lang", lang.Code());
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        /// <summary>
        /// Retrieve Map Name information
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetMapNamesTest")]
        public async static Task<EntryDictionary<int, MapNameEntry>> GetMapNamesAsync(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Map Names");
            var parser = new MapNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.map_names);
            request.AddParameter("lang", lang.Code());
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        /// <summary>
        /// Retrieve Event Details
        /// </summary>
        /// <param name="event_id">(Optional) id of event to Retrieve. Use Guid.Empty to omit event_id and retrieve details for all events</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Collection of a single <seealso cref="EventDetailsEntry"/> when event_id is null, otherwise a <seealso cref="EntryCollection{EventDetailsEntry}"/></returns>
        [Tested("GetEventDetailsTest")]
        public async static Task<EntryDictionary<Guid, EventDetailsEntry>> GetEventDetailsAsync(Guid event_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Event Details for ID: {0}", event_id.ToString());
            var parser = new EventDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.event_details);
            request.AddParameter("lang", lang.Code());
            if (event_id != Guid.Empty)
                request.AddParameter("event_id", event_id);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        /// <summary>
        /// Retrieve Event Details
        /// </summary>
        /// <param name="event_id">(Optional) id of event to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Collection of a single <seealso cref="EventDetailsEntry"/> when event_id is null, otherwise a <seealso cref="EntryCollection{EventDetailsEntry}"/></returns>
        [Tested("GetEventDetailsTest")]
        public static Task<EntryDictionary<Guid, EventDetailsEntry>> GetEventDetailsAsync(string event_id = null, bool ignoreCache = false, Language lang = Language.English)
        {
            if (event_id == null)
                return GetEventDetailsAsync(Guid.Empty, ignoreCache);
            return GetEventDetailsAsync(new Guid(event_id), ignoreCache, lang);
        }

/*        /// <summary>
        /// Retrieve Event Names
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetEventNamesTest")]
        public static EntryDictionary<Guid, EventNameEntry> GetEventNames(bool ignoreCache = false, Language lang = Language.English)
        {
            var parser = new EventNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.event_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve World Name Information
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetWorldNamesTest")]
        public static EntryDictionary<int, WorldNameEntry> GetWorldNames(bool ignoreCache = false, Language lang = Language.English)
        {
            var parser = new WorldNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.world_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Map Name information
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetMapNamesTest")]
        public static EntryDictionary<int, MapNameEntry> GetMapNames(bool ignoreCache = false, Language lang = Language.English)
        {
            var parser = new MapNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.map_names);
            request.AddParameter("lang", lang.Code());
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Event Details
        /// </summary>
        /// <param name="event_id">(Optional) id of event to Retrieve. Use Guid.Empty to omit event_id and retrieve details for all events</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Collection of a single <seealso cref="EventDetailsEntry"/> when event_id is null, otherwise a <seealso cref="EntryCollection{EventDetailsEntry}"/></returns>
        [Tested("GetEventDetailsTest")]
        public static EntryDictionary<Guid, EventDetailsEntry> GetEventDetails(Guid event_id, bool ignoreCache = false, Language lang = Language.English)
        {
            var parser = new EventDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.event_details);
            request.AddParameter("lang", lang.Code());
            if (event_id != Guid.Empty)
                request.AddParameter("event_id", event_id);
            return HandleRequest(request, parser, ignoreCache);
        }
        /// <summary>
        /// Retrieve Event Details
        /// </summary>
        /// <param name="event_id">(Optional) id of event to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Collection of a single <seealso cref="EventDetailsEntry"/> when event_id is null, otherwise a <seealso cref="EntryCollection{EventDetailsEntry}"/></returns>
        [Tested("GetEventDetailsTest")]
        public static EntryDictionary<Guid, EventDetailsEntry> GetEventDetails(string event_id = null, bool ignoreCache = false, Language lang = Language.English)
        {
            if (event_id == null)
                return GetEventDetails(Guid.Empty, ignoreCache);
            return GetEventDetails(new Guid(event_id), ignoreCache, lang);
        }*/
        #endregion Dynamic Events API - BETA

        #region Item and Recipe Database API - BETA
        [Tested("GetItemIdsTest")]
        public async static Task<IdList> GetItemIdsAsync(bool ignoreCache = false)
        {
            Logger.Info("Request for Item Id List");
            var parser = new ItemIdListParser();
            ApiRequest request = new ApiRequest(Constants.items);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        /// <summary>
        /// Retrieve List of all Recipe IDs discovered by players
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Recipe IDs discovered by players</returns>
        [Tested("GetRecipeIdsTest")]
        public async static Task<IdList> GetRecipeIdsAsync(bool ignoreCache = false)
        {
            Logger.Info("Request for Recipe Id List");
            var parser = new RecipeIdListParser();
            ApiRequest request = new ApiRequest(Constants.recipes);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        /// <summary>
        /// Retrieves the details for a given item.
        /// </summary>
        /// <param name="item_id">(Required) - Item id</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>details for the given item id</returns>
        [Tested("GetItemDetailsTest")]
        public async static Task<ItemDetailsEntry> GetItemDetailsAsync(int item_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Item: {0}", item_id);
            var parser = new ItemDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.item_details);
            request.AddParameter("lang", lang.Code());
            request.AddParameter("item_id", item_id);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve Recipe information
        /// </summary>
        /// <param name="recipe_id">(Required) id of recipe to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Recipe information</returns>
        [Tested("GetRecipeDetailsTest")]
        public async static Task<RecipeDetailsEntry> GetRecipeDetailsAsync(int recipe_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Recipe: {0}", recipe_id);
            var parser = new RecipeDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.recipe_details);
            request.AddParameter("lang", lang.Code());
            request.AddParameter("recipe_id", recipe_id);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        #endregion Item and Recipe Database API - BETA

        #region Guild API – BETA

        /// <summary>
        /// Retrieve Guild Information for a given Guild Name
        /// </summary>
        /// <param name="guild_name">(Required) Name of guild to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Guild information</returns>
        [Tested("GetGuildDetailsByNameTest")]
        public async static Task<GuildDetailsEntry> GetGuildDetailsByNameAsync(string guild_name, bool ignoreCache = false)
        {
            Logger.Info("Request for {0} - {1}", typeof (GuildDetailsEntry).Name, guild_name);
            var parser = new GuildDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.guild_details);
            request.AddParameter("guild_name", guild_name);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve Guild Information for a given Guild ID
        /// </summary>
        /// <param name="guild_id">(Required) id of guild to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Guild information</returns>
        [Tested("GetGuildDetailsByIdTest")]
        public static Task<GuildDetailsEntry> GetGuildDetailsByIdAsync(string guild_id, bool ignoreCache = false)
        {
            return GetGuildDetailsByIdAsync(new Guid(guild_id), ignoreCache);
        }

        /// <summary>
        /// Retrieve Guild Information for a given Guild ID
        /// </summary>
        /// <param name="guild_id">(Required) id of guild to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Guild information</returns>
        [Tested("GetGuildDetailsByIdTest")]
        public async static Task<GuildDetailsEntry> GetGuildDetailsByIdAsync(Guid guild_id, bool ignoreCache = false)
        {
            Logger.Info("Request for {0} - {1}", typeof(GuildDetailsEntry).Name, guild_id.ToString());
            var parser = new GuildDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.guild_details);
            request.AddParameter("guild_id", guild_id);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        #endregion Guild API – BETA

        #region WvW API - BETA
        /// <summary>
        /// Retrieve a list of Matches
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>List of <seealso cref="MatchEntry"/></returns>
        [Tested("GetMatchesTest")]
        public async static Task<EntryDictionary<string, MatchEntry>> GetMatchesAsync(bool ignoreCache = false)
        {
            Logger.Info("Request for Matches");
            var parser = new MatchEntryParser();
            ApiRequest request = new ApiRequest(Constants.wvw_matches);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve Match Details
        /// </summary>
        /// <param name="match_id">(Required) id of the match to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="MatchDetailsEntry"/> containing information for a given match</returns>
        [Tested("GetMatchDetailsTest")]
        public async static Task<MatchDetailsEntry> GetMatchDetailsAsync(string match_id, bool ignoreCache = false)
        {
            Logger.Info("Request for Match ID: {0}", match_id);
            var parser = new MatchDetailsEntryParser();
            ApiRequest request = new ApiRequest(Constants.wvw_match_details);
            request.AddParameter("match_id", match_id);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Get WvW ObjectiveNames
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>ObjectiveNames</returns>
        [Tested("GetObjectiveNamesTest")]
        public async static Task<EntryDictionary<int, ObjectiveNameEntry>> GetObjectiveNamesAsync(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Objective Names");
            var parser = new ObjectiveNameEntryParser();
            ApiRequest request = new ApiRequest(Constants.wvw_objective_names);
            request.AddParameter("lang", lang.Code());
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        #endregion WvW API - BETA

        #region Map API - BETA
        /// <summary>
        /// Retrieve a list of contintents.
        /// <remarks>Indexed by continent ID</remarks>
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetContinentsTest")]
        public async static Task<EntryDictionary<int, ContinentEntry>> GetContinentsAsync(bool ignoreCache = false)
        {
            Logger.Info("Request for Continents");
            var parser = new ContinentEntryParser();
            ApiRequest request = new ApiRequest(Constants.continents);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve Map Entries
        /// <remarks>Indexed by Map ID</remarks>
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="EntryCollection{MapEntry}"/></returns>
        [Tested("GetMapTest")]
        public static Task<EntryDictionary<int, MapEntry>> GetMapAsync(bool ignoreCache = false, Language lang = Language.English)
        {
            return GetMapAsync(-1, ignoreCache, lang);
        }

        /// <summary>
        /// Retrieve Map Entries
        /// <remarks>Indexed by Map ID</remarks>
        /// </summary>
        /// <param name="map_id">(Optional) Map id of the map entry to Retrieve</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="EntryCollection{MapEntry}"/></returns>
        [Tested("GetMapTest")]
        public async static Task<EntryDictionary<int, MapEntry>> GetMapAsync(int map_id, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Map Entry ID: {0}", map_id);
            var parser = new MapEntryParser();
            ApiRequest request = new ApiRequest(Constants.maps);
            request.AddParameter("lang", lang.Code());
            if (map_id > 0) request.AddParameter("map_id", map_id);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Map Floor information. Useful when using the world map tile service to display maps.
        /// </summary>
        /// <param name="continent_id">(Required) Continent to obtain info for</param>
        /// <param name="floor">(Required) floor of continent, check <seealso cref="ContinentEntry"/> for a list of floors</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns><seealso cref="MapFloorEntry"/></returns>
        [Tested("GetMapFloorTest")]
        public async static Task<MapFloorEntry> GetMapFloorAsync(int continent_id, int floor, bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Map floor {0} on Continent {1}", floor, continent_id);
            var parser = new MapFloorEntryParser();
            ApiRequest request = new ApiRequest(Constants.map_floor);
            request.AddParameter("lang", lang.Code());
            request.AddParameter("continent_id", continent_id);
            request.AddParameter("floor", floor);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }
        #endregion Map API - BETA

        #region Miscellaneous APIs - BETA
        /// <summary>
        /// Get Colors used for Guild Emblems
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>Color Details</returns>
        [Tested("GetColorsTest")]
        public async static Task<EntryDictionary<int, ColorEntry>> GetColorsAsync(bool ignoreCache = false, Language lang = Language.English)
        {
            Logger.Info("Request for Colors");
            var parser = new ColorEntryParser();
            ApiRequest request = new ApiRequest(Constants.colors);
            request.AddParameter("lang", lang.Code());
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrives a list of files that can be used with the Render Services api.
        /// Provides icons and images for maps, items, etc..
        /// </summary>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetFilesTest")]
        public async static Task<EntryDictionary<string, FileEntry>> GetFilesAsync(bool ignoreCache = false)
        {
            Logger.Info("Request for File Entries");
            var parser = new FileEntryParser();
            ApiRequest request = new ApiRequest(Constants.files);
            return await HandleRequestAsync(request, parser, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// The render service provides access to in-game assets like item icons.
        /// </summary>
        /// <param name="signature">(Required) File signature (Check <seealso cref="ItemDetailsEntry"/> or <seealso cref="FileEntry"/> for valid signatures)</param>
        /// <param name="file_id">(Required) File ID (Check <seealso cref="ItemDetailsEntry"/> or <seealso cref="FileEntry"/> for valid file id's)</param>
        /// <param name="format">(Required) Image format, Currently .png or .jpg</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetRenderServiceAssetEntryTest")]
        public static Task<RenderServiceAssetEntry> GetRenderServiceAssetEntryAsync(string signature, int file_id, string format, bool ignoreCache = false)
        {
            return GetRenderServiceAssetEntryAsync(new FileEntry() { Signature = signature, FileID = file_id }, format, ignoreCache);
        }

        /// <summary>
        /// The render service provides access to in-game assets like item icons.
        /// </summary>
        /// <param name="file">(Required) File entry contining a signature and id</param>
        /// <param name="format">(Required) Image format, Currently .png or .jpg</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns></returns>
        [Tested("GetRenderServiceAssetEntryTest")]
        public async static Task<RenderServiceAssetEntry> GetRenderServiceAssetEntryAsync(FileEntry file, string format, bool ignoreCache = false)
        {
            Logger.Info("Request for {0} File:{1} Sig:{2}", format, file.FileID, file.Signature);
            var parser = new RenderServiceAssetParser();
            IApiRequest request = new ApiRequest(
                string.Format(Constants.RenderServiceUriFormat, file.Signature, file.FileID, format));
            RenderServiceAssetEntry entry = await HandleRequestAsync(request, parser, RenderServiceNetwork, ignoreCache).ConfigureAwait(false);
            entry.File = file;
            return entry;
        }
        #endregion Miscellaneous APIs - BETA

        #endregion Async Api Calls

        #region Request Handling

        /// <summary>
        /// Handles a request to the <see cref="IApiResponseParser{T}" />.
        /// </summary>
        /// <param name="request">The <see cref="IApiRequest" />.</param>
        /// <param name="parser">The <see cref="IApiResponseParser{T}" /> which should be used to parse the response.</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>The appropriate (parsed) <see cref="IApiRequest" /></returns>
        internal static T HandleRequest<T>(IApiRequest request, IApiResponseParser<T> parser, bool ignoreCache)
            where T : ResponseObject
        {
            return HandleRequest(request, parser, Network, ignoreCache);
        }

        /// <summary>
        /// Handles a request to the <see cref="IApiResponseParser{T}" />.
        /// </summary>
        /// <param name="request">The <see cref="IApiRequest" />.</param>
        /// <param name="parser">The <see cref="IApiResponseParser{T}" /> which should be used to parse the response.</param>
        /// <param name="network">The <see cref="INetworkHandler"/> used for executing requests.</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>The appropriate (parsed) <see cref="IApiRequest" /></returns>
        internal static T HandleRequest<T>(IApiRequest request, IApiResponseParser<T> parser, INetworkHandler network,
                                           bool ignoreCache) where T : ResponseObject
        {
            try
            {
                var requestHandler = new ApiRequestHandler<T>(parser, network, ignoreCache);
                return requestHandler.HandleRequest(request);
            }
            catch (Exception e)
            {
                    if (HandleError(ErrorHandlers[e.GetType()] as Action<object>, e) == false)
                        throw;
            }
            return null;
        }

        public static void RegisterErrorHandler<T>(Action<object> errorHandler)
        {
            ErrorHandlers.Add(typeof (T), errorHandler);
        }

        internal static Hashtable ErrorHandlers = new Hashtable();

        internal static bool HandleError(Action<object> handler, Exception e )
        {
            if (handler != null)
            {
                handler(e);
                return true;
            }
            return false;
        }

        #endregion Request Handling

        #region Request Handling Async

        /// <summary>
        /// Handles a request to the <see cref="IApiResponseParser{T}" />.
        /// </summary>
        /// <param name="request">The <see cref="IApiRequest" />.</param>
        /// <param name="parser">The <see cref="IApiResponseParser{T}" /> which should be used to parse the response.</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>The appropriate (parsed) <see cref="IApiRequest" /></returns>
        internal static Task<T> HandleRequestAsync<T>(IApiRequest request, IApiResponseParserAsync<T> parser, bool ignoreCache)
            where T : ResponseObject
        {
            return HandleRequestAsync(request, parser, Network, ignoreCache);
        }

        /// <summary>
        /// Handles a request to the <see cref="IApiResponseParser{T}" />.
        /// </summary>
        /// <param name="request">The <see cref="IApiRequest" />.</param>
        /// <param name="parser">The <see cref="IApiResponseParser{T}" /> which should be used to parse the response.</param>
        /// <param name="network">The <see cref="INetworkHandler"/> used for executing requests.</param>
        /// <param name="ignoreCache"><code>
        /// true: ignores cache and Retrieves the item details via the GW2 api.
        /// false: use cached data if it is available and not expired</code></param>
        /// <returns>The appropriate (parsed) <see cref="IApiRequest" /></returns>
        internal static Task<T> HandleRequestAsync<T>(IApiRequest request, IApiResponseParserAsync<T> parser, INetworkHandler network,
                                           bool ignoreCache) where T : ResponseObject
        {
            try
            {
                var requestHandler = new ApiRequestHandler<T>(parser, network, ignoreCache);
                return requestHandler.HandleRequestAsync(request);
            }
            catch (Exception e)
            {
                if (HandleError(ErrorHandlers[e.GetType()] as Action<object>, e) == false)
                    throw;
            }
            return null;
        }

        #endregion Request Handling Async
    }
}
