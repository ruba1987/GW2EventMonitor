using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for guild_details.json
    /// </summary>
    public class GuildDetailsEntryParser : IApiResponseParserAsync<GuildDetailsEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public GuildDetailsEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public GuildDetailsEntry Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return Verify(ParserHelper<GuildDetailsEntry>.Parse(json), json);
        }

        protected virtual GuildDetailsEntry Verify(GuildDetailsEntry entry, string response)
        {
            if (entry.GuildName == null || entry.GuildId == Guid.Empty)
            {
                throw ExceptionHelper.ResponseError(response, "Error Retrieving Guild Details.  Bad guild name or id\n");
            }

            return entry;
        }

        public async Task<GuildDetailsEntry> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return Verify(await ParserHelper<GuildDetailsEntry>.ParseAsync(json).ConfigureAwait(false), json);
        }
    }
}
