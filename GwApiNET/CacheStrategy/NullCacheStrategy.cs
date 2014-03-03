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
    [KnownType(typeof(NullCacheStrategy))]
    public class NullCacheStrategy : ICacheStrategy
    {
        public static ICacheStrategy NullStrategy = new NullCacheStrategy(false);
        bool ExpiredValue { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public NullCacheStrategy(bool expired)
        {
            ExpiredValue = expired;
        }

        public bool Expired(ResponseObject responseObject)
        {
            return ExpiredValue;
        }
    }
}
