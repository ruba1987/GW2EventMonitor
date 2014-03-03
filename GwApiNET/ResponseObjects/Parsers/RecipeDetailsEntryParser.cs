using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    public class RecipeDetailsEntryParser : IApiResponseParserAsync<RecipeDetailsEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecipeDetailsEntryParser()
        {
        }

        public RecipeDetailsEntry Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return ParserHelper<RecipeDetailsEntry>.Parse(json);
        }

        public async Task<RecipeDetailsEntry> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return await ParserHelper<RecipeDetailsEntry>.ParseAsync(json).ConfigureAwait(false);
        }
    }
}
