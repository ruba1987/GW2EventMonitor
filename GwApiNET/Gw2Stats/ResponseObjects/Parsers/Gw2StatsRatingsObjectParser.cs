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
    /// Gw2Stats.net parser for ratings.json
    /// </summary>
    public class Gw2StatsRatingsObjectParser : IApiResponseParser<Gw2StatsRatingsObject>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsRatingsObjectParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public Gw2StatsRatingsObject Parse(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return null;
            string json = response.Content;
            var temp = ParserHelper<Gw2StatsRatingsObject>.Parse(json);
            return temp;
        }
    }
}
