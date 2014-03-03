using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET.CacheStrategy
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(AgeCacheStrategy))]
    public class AgeCacheStrategy : ICacheStrategy
    {
        [DataMember]
        public TimeSpan MaxAge { get; set; }

        /// <summary>
        /// Constructor with 30 second maximum age.
        /// </summary>
        public AgeCacheStrategy() : this(TimeSpan.FromSeconds(30))
        {}
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AgeCacheStrategy(TimeSpan maxAge)
        {
            MaxAge = maxAge;
        }

        public bool Expired(ResponseObject responseObject)
        {
            return Expired(responseObject.Age);
        }

        public bool Expired(TimeSpan age)
        {
            return age >= MaxAge;
        }
    }
}
