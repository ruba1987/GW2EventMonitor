using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using GwApiNET.ResponseObjects.Parsers;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GwApiNET.Gw2Stats.ResponseObjects.Parsers
{

    public class Gw2StatsStatusCodeParser : IApiResponseParser<StatusCodeDescriptions>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsStatusCodeParser()
        {
            
        }

        public StatusCodeDescriptions Parse(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return null;
            string json = response.Content;
            var temp = ParserHelper<Dictionary<string, EntryDictionary<string, JObject>>>.Parse(json);
            StatusCodeDescriptions status_codes = new StatusCodeDescriptions();
            foreach (var obj in temp["status_codes"])
            {
                status_codes.Add(obj.Key, obj.Value["description"].ToString());
            }
            return status_codes;
        }
    }
}
