using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for event_names.json
    /// </summary>
    public class EventNameEntryParser : IApiResponseParserAsync<EntryDictionary<Guid,EventNameEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EventNameEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<Guid,EventNameEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            EntryCollection<EventNameEntry> entries = ParserHelper<EntryCollection<EventNameEntry>>.Parse(json);
            var dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }

        public async Task<EntryDictionary<Guid, EventNameEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            EntryCollection<EventNameEntry> entries = await ParserHelper<EntryCollection<EventNameEntry>>.ParseAsync(json).ConfigureAwait(false);
            var dict = entries.ToEntryDictionary(e => e.Id);
            return dict;
        }
    }
}
