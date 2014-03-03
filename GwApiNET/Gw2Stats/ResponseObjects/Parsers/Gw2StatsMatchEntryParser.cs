using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects.Parsers;
using RestSharp;

namespace GwApiNET.Gw2Stats.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for gw2stats.net matches.json
    /// </summary>
    public class Gw2StatsMatchEntryParser : IApiResponseParser<Gw2StatsMatchEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsMatchEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public Gw2StatsMatchEntry Parse(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return null;
            string json = response.Content;
            var entries = ParserHelper<Gw2StatsMatchEntry>.Parse(json);
            foreach (var region in entries.Region)
            {
                foreach (var reg in region.Value)
                {
                    reg.RegionId = region.Key;
                    reg.Worlds = reg._Worlds.ToDictionary(w => w.Id);
                }
            }
            return entries;
        }
    }
}
