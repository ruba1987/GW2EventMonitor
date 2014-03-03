using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    /// <summary>
    /// Parser for match_details.json
    /// </summary>
    public class MatchDetailsEntryParser : IApiResponseParserAsync<MatchDetailsEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MatchDetailsEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public MatchDetailsEntry Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return ParserHelper<MatchDetailsEntry>.Parse(json);
        }

        public async Task<MatchDetailsEntry> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return await ParserHelper<MatchDetailsEntry>.ParseAsync(json).ConfigureAwait(false);
        }
    }
}
