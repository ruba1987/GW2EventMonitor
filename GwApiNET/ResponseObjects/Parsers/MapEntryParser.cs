using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for maps.json
    /// </summary>
    public class MapEntryParser : IApiResponseParserAsync<EntryDictionary<int, MapEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<int, MapEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var maps2 = ParserHelper<Dictionary<string, EntryDictionary<int, MapEntry>>>.Parse(json);
            if (maps2 != null)
            {
                var maps = maps2["maps"];
                foreach (var map in maps)
                {
                    map.Value.Id = map.Key;
                }
                return maps;
            }
            return new EntryDictionary<int,MapEntry>();
        }

        public async Task<EntryDictionary<int, MapEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var maps2 = await ParserHelper<Dictionary<string, EntryDictionary<int, MapEntry>>>.ParseAsync(json).ConfigureAwait(false);
            EntryDictionary<int, MapEntry> maps = null;
            if (maps2 != null)
            {
                maps = maps2["maps"];
                foreach (var map in maps)
                {
                    map.Value.Id = map.Key;
                }
            }
            return maps ?? new EntryDictionary<int, MapEntry>();
        }
    }
}
