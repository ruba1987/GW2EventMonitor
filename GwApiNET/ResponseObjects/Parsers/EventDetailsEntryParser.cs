using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for event_details.json
    /// </summary>
    public class EventDetailsEntryParser : IApiResponseParserAsync<EntryDictionary<Guid,EventDetailsEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EventDetailsEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<Guid,EventDetailsEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var obj = ParserHelper<Dictionary<string, EntryDictionary<Guid, EventDetailsEntry>>>.Parse(json);
            foreach (var pair in obj["events"])
            {
                pair.Value.EventId = pair.Key;
            }
            return obj["events"];
        }

        public async Task<EntryDictionary<Guid, EventDetailsEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var obj = await ParserHelper<Dictionary<string, EntryDictionary<Guid, EventDetailsEntry>>>.ParseAsync(json).ConfigureAwait(false);
            return await Task.Run(() =>
                {
                    foreach (var pair in obj["events"])
                    {
                        pair.Value.EventId = pair.Key;
                    }
                    return obj["events"];
                }).ConfigureAwait(false);
        }
    }
}
