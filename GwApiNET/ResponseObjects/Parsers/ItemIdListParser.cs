using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Praser for items.json
    /// </summary>
    public class ItemIdListParser : IApiResponseParserAsync<IdList>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ItemIdListParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public IdList Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var itemIdDict = ParserHelper<Dictionary<string, List<int>>>.Parse(json);
            List<int> itemIds = itemIdDict["items"];
            return new IdList(itemIds);
        }

        public async Task<IdList> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var itemIdDict = await ParserHelper<Dictionary<string, List<int>>>.ParseAsync(json);
            List<int> itemIds = itemIdDict["items"];
            return new IdList(itemIds);
        }
    }
}
