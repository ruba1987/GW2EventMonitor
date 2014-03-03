using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    /// <summary>
    /// Parse match_entry.json response object
    /// </summary>
    public class MatchEntryParser : IApiResponseParserAsync<EntryDictionary<string, MatchEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MatchEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<string, MatchEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = ParserHelper<Dictionary<string, EntryCollection<MatchEntry>>>.Parse(json)["wvw_matches"];
            var dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }

        public async Task<EntryDictionary<string, MatchEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = await ParserHelper<Dictionary<string, EntryCollection<MatchEntry>>>.ParseAsync(json).ConfigureAwait(false);
            var dict = entries["wvw_matches"].ToEntryDictionary(e => e.Id);
            return dict;
        }
    }
}
