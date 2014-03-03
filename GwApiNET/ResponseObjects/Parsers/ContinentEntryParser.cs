using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for continents.json
    /// </summary>
    public class ContinentEntryParser : IApiResponseParserAsync<EntryDictionary<int,ContinentEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ContinentEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public EntryDictionary<int,ContinentEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var dictionary = ParserHelper<Dictionary<string, EntryDictionary<int, ContinentEntry>>>.Parse(json);
            try
            {
                var continents = dictionary["continents"];

                foreach (var pair in continents)
                {
                    pair.Value.Id = pair.Key;
                }
                GwApi.Logger.Info("Parsed {0} Colors", "ID:" + string.Join(",", continents.Keys));

                return continents;
            }
            catch (Exception e)
            {
                GwApi.Logger.Error(e);
            }
            return new EntryDictionary<int, ContinentEntry>();
        }

        public async Task<EntryDictionary<int, ContinentEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var dictionary = await ParserHelper<Dictionary<string, EntryDictionary<int, ContinentEntry>>>.ParseAsync(json).ConfigureAwait(false);
            EntryDictionary<int, ContinentEntry> continents = null;
            try
            {
                continents = dictionary["continents"];

                foreach (var pair in continents)
                {
                    pair.Value.Id = pair.Key;
                }
                GwApi.Logger.Info("Parsed {0} Colors - {1}",
                                  "ID:" + string.Join(",", continents.Keys), Thread.CurrentContext.ContextID);
            }
            catch (Exception e)
            {
                GwApi.Logger.Error(e);
            }
            return continents ?? new EntryDictionary<int, ContinentEntry>();
        }
    }
}
