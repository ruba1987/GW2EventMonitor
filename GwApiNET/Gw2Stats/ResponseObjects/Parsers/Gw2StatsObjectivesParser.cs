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
    /// Parser for gw2stats.net status.json
    /// </summary>
    public class Gw2StatsObjectivesParser : IApiResponseParser<Gw2StatsObjectives>
    {
        /// <summary>
        /// Expected object type.
        /// </summary>
        public Gw2StatsObjectives.ObjectiveType Type { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Gw2StatsObjectivesParser(Gw2StatsObjectives.ObjectiveType type)
        {
            Type = type;
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public Gw2StatsObjectives Parse(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return null;
            string json = response.Content;
            Gw2StatsObjectives objectives = null;
            switch (Type)
            {
                case Gw2StatsObjectives.ObjectiveType.All:
                    objectives = ParserHelper<Gw2StatsObjectives>.Parse(json);
                    break;
                case Gw2StatsObjectives.ObjectiveType.Match:
                    var obm = ParserHelper<_ObjectivesByMatch>.Parse(json);
                    objectives = new Gw2StatsObjectives(obm);
                    break;
                case Gw2StatsObjectives.ObjectiveType.World:
                    var obw = ParserHelper<_ObjectivesByWorld>.Parse(json);
                    objectives = new Gw2StatsObjectives(obw);
                    break;
            }
            return objectives;
        }
    }
}
