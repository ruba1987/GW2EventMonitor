using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    public class ItemDetailsEntryParser : IApiResponseParserAsync<ItemDetailsEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ItemDetailsEntryParser()
        {
        }

        public ItemDetailsEntry Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            JsonSerializer serializer = new JsonSerializer();
            using (TextReader reader = new StringReader(json))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    try
                    {
                        serializer.Converters.Add(new StringEnumConverter());
                        var itemDetails = serializer.Deserialize<ItemDetailsEntry>(jreader);
                        return itemDetails;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        GwApi.Logger.Error(e);
                    }
                    return null;
                }
            }
        }

        public async Task<ItemDetailsEntry> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return await ParserHelper<ItemDetailsEntry>.ParseAsync(json).ConfigureAwait(false);
        }
    }
}
