using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parse World Name information.
    /// <remarks>world_name.json parser</remarks>
    /// </summary>
    public class WorldNameEntryParser : IApiResponseParserAsync<EntryDictionary<int,WorldNameEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public WorldNameEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<int,WorldNameEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = ParserHelper<EntryCollection<WorldNameEntry>>.Parse(json);
            EntryDictionary<int, WorldNameEntry> dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }

        public async Task<EntryDictionary<int, WorldNameEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = await ParserHelper<EntryCollection<WorldNameEntry>>.ParseAsync(json).ConfigureAwait(false);
            EntryDictionary<int, WorldNameEntry> dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }
    }
}
