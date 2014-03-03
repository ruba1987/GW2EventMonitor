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
    [KnownType(typeof(BuildVersionCacheStrategy))]
    public class BuildVersionCacheStrategy : ICacheStrategy
    {
        [DataMember]
        public BuildVersionCondition Condition { get; set; }

        public BuildVersionCacheStrategy() : this(BuildVersionCondition.Changes)
        {}

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BuildVersionCacheStrategy(BuildVersionCondition condition = BuildVersionCondition.Changes)
        {
            Condition = condition;
        }

        public enum BuildVersionCondition
        {
            Changes,
        }

        public bool Expired(ResponseObject responseObject)
        {
            return Expired(responseObject, GwApi.Build);
        }

        public bool Expired(ResponseObject responseObject, int currentBuild)
        {
            switch (Condition)
            {
                case BuildVersionCondition.Changes:
                    return responseObject.LastUpdateBuild != currentBuild;
                default:
                    return true;
            }
            
        }
    }
}
