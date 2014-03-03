using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GwApiNET.CacheStrategy;
using RestSharp;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Represents the base class for all responses from the GW2 API
    /// </summary>
    [Serializable]
    public class ResponseObject
    {
        /// <summary>
        /// Url used to retrieve the object from the GW2 api
        /// </summary>
        public string Url { get; set; }
        private int _lastUpdateBuild = GwApi.Build;
        /// <summary>
        /// The build of GW2 when this object was last retrieved/updated
        /// </summary>
        public int LastUpdateBuild { get { return _lastUpdateBuild; } set { _lastUpdateBuild = value; } }
        // backer for initialization
        private DateTime _lastUpdated = DateTime.Now;
        /// <summary>
        /// The DateTime when the object was retrieved/updated
        /// </summary>
        public DateTime LastUpdated { get { return _lastUpdated; } set { _lastUpdated = value; } }

        /// <summary>
        /// Amount of time since the object was retrieved
        /// </summary>
        [XmlIgnore]
        public TimeSpan Age { get { return DateTime.Now - LastUpdated; } }

        private ICacheStrategy _cacheStrategy = null;
        /// <summary>
        /// Caching strategy used by the ResourceCache to determine if the object has expired.
        /// </summary>
        public virtual ICacheStrategy CacheStrategy { get { return _cacheStrategy ?? Constants.GetCacheStrategy(this); } set { _cacheStrategy = value; } }
        /// <summary>
        /// Identifies weather this object is from cache (true) or from the GW2 servers (false)
        /// </summary>
        public bool FromCache { get; internal protected set; }

        /// <summary>
        /// Identifies weather the response object has expired
        /// </summary>
        [XmlIgnore]
        public virtual bool Expired
        {
            get { return CacheStrategy != null ? CacheStrategy.Expired(this) : NullCacheStrategy.NullStrategy.Expired(this); }
        }

        /// <summary>
        /// original response value from GW2 API
        /// </summary>
        [XmlIgnore]
        public object RawResponse { get; set; }

        public virtual void SetResponse(object response)
        {
            RawResponse = response is IRestResponse
                           ? (response as IRestResponse).Content
                           : response;
        }
    }
}
