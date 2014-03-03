using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    /// <summary>
    /// Parse recipes.json response object
    /// </summary>
    public class RecipeIdListParser : IApiResponseParserAsync<IdList>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecipeIdListParser()
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
            var recipesDict = ParserHelper<Dictionary<string, List<int>>>.Parse(json);
            List<int> recipeIds = recipesDict["recipes"];
            return new IdList(recipeIds);
        }

        public async Task<IdList> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var recipesDict = await ParserHelper<Dictionary<string, List<int>>>.ParseAsync(json).ConfigureAwait(false);
            List<int> recipeIds = recipesDict["recipes"];
            return new IdList(recipeIds);
        }
    }
}
