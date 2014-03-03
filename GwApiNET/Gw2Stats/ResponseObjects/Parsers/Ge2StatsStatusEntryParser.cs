using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using GwApiNET.ResponseObjects.Parsers;
using RestSharp;

namespace GwApiNET.Gw2Stats.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for gw2stats.net status.json
    /// </summary>
    public class Ge2StatsStatusEntryParser : IApiResponseParser<EntryDictionary<string, Gw2StatsStatusEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Ge2StatsStatusEntryParser()
        {
        }

        public EntryDictionary<string, Gw2StatsStatusEntry> Parse(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return null;
            string json = response.Content;
            var temp = ParserHelper<Dictionary<string, EntryDictionary<string, Gw2StatsStatusEntry>>>.Parse(json);
            EntryDictionary<string, Gw2StatsStatusEntry> apiStatuses = temp["api"];

            foreach (var status in apiStatuses)
            {
                status.Value.Id = status.Key;
            }
            return apiStatuses;
        }
    }
}
