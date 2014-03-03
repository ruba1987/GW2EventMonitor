using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    /// <summary>
    /// Parse objective_name.json response object
    /// </summary>
    public class ObjectiveNameEntryParser : IApiResponseParserAsync<EntryDictionary<int, ObjectiveNameEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ObjectiveNameEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<int,ObjectiveNameEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = ParserHelper<EntryCollection<ObjectiveNameEntry>>.Parse(json);
            var dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }

        public async Task<EntryDictionary<int, ObjectiveNameEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var entries = await ParserHelper<EntryCollection<ObjectiveNameEntry>>.ParseAsync(json).ConfigureAwait(false);
            var dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }
    }
}
