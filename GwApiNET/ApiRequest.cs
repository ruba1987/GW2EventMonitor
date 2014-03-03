using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET
{

    public class ApiRequest : IApiRequest
    {
        private static RestClient Client = new RestClient();
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiRequest() : this("")
        {
        }

        public ApiRequest(string resourceUrl)
        {
            Parameters = new Dictionary<string, object>();
            Resource = resourceUrl;
        }

        public string Resource { get; set; }
        public void AddParameter(string name, object value)
        {
            Parameters[name] = value;
        }

    }
}
