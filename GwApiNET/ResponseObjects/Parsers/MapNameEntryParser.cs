using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for map_names.json
    /// </summary>
    public class MapNameEntryParser : IApiResponseParserAsync<EntryDictionary<int,MapNameEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapNameEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<int, MapNameEntry> Parse(object apiResponse)
        {
            var task = ParseAsync(apiResponse);
            task.Wait();
            return task.Result;
        }

        public async Task<EntryDictionary<int, MapNameEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = await ParserHelper<EntryCollection<MapNameEntry>>.ParseAsync(json).ConfigureAwait(false);
            var dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }
    }
}
