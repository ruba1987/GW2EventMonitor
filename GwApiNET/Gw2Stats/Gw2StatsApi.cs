using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.Gw2Stats.ResponseObjects;
using GwApiNET.Gw2Stats.ResponseObjects.Parsers;
using GwApiNET.ResponseObjects;
using GwApiNET.ResponseObjects.Parsers;

namespace GwApiNET.Gw2Stats
{

    public static class Gw2StatsApi
    {
        private static StatusCodeDescriptions _statusCodes;
        public static StatusCodeDescriptions StatusCodes
        {
            get { return _statusCodes ?? (_statusCodes = GetGw2StatsStatusCodes()); }
            internal set { _statusCodes = value; }
        }

        public static INetworkHandler Network;
        
        static Gw2StatsApi()
        {
            Network = new NetworkHandler(Language.None, Constants.gw2_ApiBaseUrl);
        }

        public static EntryDictionary<string,Gw2StatsStatusEntry> GetGw2StatsStatus(bool ignoreCache = true)
        {
            var parser = new Ge2StatsStatusEntryParser();
            ApiRequest request = new ApiRequest(Constants.gw2_status);
            return GwApi.HandleRequest(request, parser, Network, ignoreCache);
        }

        public static StatusCodeDescriptions GetGw2StatsStatusCodes(bool ignoreCache = false)
        {
            var parser = new Gw2StatsStatusCodeParser();
            ApiRequest request = new ApiRequest(Constants.gw2_status_codes);
            var codes = GwApi.HandleRequest(request, parser, Network, ignoreCache);
            _statusCodes = _statusCodes ?? new StatusCodeDescriptions();
            _statusCodes.Clear();
            _statusCodes.AddRange(codes);
            return codes;
        }

        public static Gw2StatsMatchEntry GetGw2StatsMatchEntry(bool objectives = false, bool ratings = false, bool ignoreCache = true)
        {
            var parser = new Gw2StatsMatchEntryParser();
            ApiRequest request = new ApiRequest(Constants.gw2_matches);
            request.AddParameter("objectives", objectives ? "true" : "false");
            request.AddParameter("ratings", ratings ? "true" : "false");
            return GwApi.HandleRequest(request, parser, Network, ignoreCache);
        }

        public static Gw2StatsObjectives GetGw2StatsObjectives(Gw2StatsObjectives.ObjectiveType type, int id,
                                                               bool ignoreCache = true)
        {
            return GetGw2StatsObjectives(type, id.ToString(), ignoreCache);
        }

        public static Gw2StatsObjectives GetGw2StatsObjectives(Gw2StatsObjectives.ObjectiveType type, string id,
                                                           bool ignoreCache = true)
        {
            var parser = new Gw2StatsObjectivesParser(type);
            ApiRequest request = new ApiRequest(Constants.gw2_objectives);
            request.AddParameter("type", type.ToString().ToLower());
            if (type != Gw2StatsObjectives.ObjectiveType.All)
                request.AddParameter("id", id);
            return GwApi.HandleRequest(request, parser, Network, ignoreCache);
        }

        public static Gw2StatsRatingsObject Gw2StatsRatingsObject(bool ignoreCache = true)
        {
            var parser = new Gw2StatsRatingsObjectParser();
            ApiRequest request = new ApiRequest(Constants.gw2_ratings);
            return GwApi.HandleRequest(request, parser, Network, ignoreCache);
        }
    }
}
