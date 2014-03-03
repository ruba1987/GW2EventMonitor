using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RestSharp;

namespace GwApiNET
{
    /// <summary>
    /// Handles basic GW2 object requests.
    /// </summary>
    public class NetworkHandler : INetworkHandler
    {
        private RestClient _client;
        private Language _lang;
        /// <summary>
        /// Base/root url to remote objects.
        /// </summary>
        public string BaseUrl { get { return _client.BaseUrl; } set { _client.BaseUrl = value; } }
        /// <summary>
        /// Build a full URI using the given IApiRequest
        /// </summary>
        /// <param name="request">request to build the full Uri with</param>
        /// <returns>fully qualified Uri</returns>
        public string BuildUri(IApiRequest request)
        {
            return _client.BuildUri(BuildRequest(request)).ToString();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        [Obsolete("The language parameter is no longer used for NetworkHanlder.  The language is not specified for each api call seperatly.")]
        public NetworkHandler(Language lang = Language.English, string baseUrl = null) : this(baseUrl)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseUrl">base url resource</param>
        public NetworkHandler(string baseUrl)
        {
            _client = new RestClient(baseUrl ?? Constants.ApiBaseUrl);
        }
        /// <summary>
        /// Retrives the object for the given request.
        /// </summary>
        /// <param name="request">API Request object used to build the resource url</param>
        /// <returns>requested object of type T</returns>
        public virtual object GetResponse(IApiRequest request)
        {
            return GetResponse(BuildRequest(request));
        }
        /// <summary>
        /// Retrives the object for the given request.
        /// </summary>
        /// <typeparam name="T">Type of requested object</typeparam>
        /// <param name="request">API Request object used to build the resource url</param>
        /// <returns>requested object of type T</returns>
        public virtual T GetResponse<T>(IApiRequest request)
        {
            return (T)GetResponse(request);
        }

        /// <summary>
        /// Retrives the object for the given request.
        /// </summary>
        /// <param name="request">API Request object used to build the resource url</param>
        /// <returns>requested object of type T</returns>
        protected virtual object GetResponse(IRestRequest request)
        {
            try
            {
                //request.AddParameter("lang", _lang.Code());
                GwApi.Logger.Info("Obtaining response for " + request.Resource);
                var response = _client.Execute(request);
                return response;
            }
            catch (Exception e)
            {
                GwApi.Logger.Error(e);
            }
            return "";
        }

        /// <summary>
        /// Build an <seealso cref="IRestRequest"/> 
        /// </summary>
        /// <param name="request">API Request used to build an IRestRequest</param>
        /// <returns></returns>
        internal static IRestRequest BuildRequest(IApiRequest request)
        {
            RestRequest restRequest = new RestRequest(request.Resource, Method.GET);
            foreach (var pair in request.Parameters)
            {
                restRequest.AddParameter(pair.Key, pair.Value);
            }
            return restRequest;
        }


    }

}
