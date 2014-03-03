using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET.CacheStrategy
{

    public class CollectionCacheStrategy<T, TItem> : ICacheStrategy where T : EntryCollection<TItem> where TItem : ResponseObject
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CollectionCacheStrategy()
        {
        }

        public bool Expired(ResponseObject responseObject)
        {
            var list = responseObject as T;
            if (list != null)
            {
                if (list.Count == 0) return true;
                return list.First().CacheStrategy.Expired(responseObject);
            }
            return false;
        }

        public bool Expired(EntryCollection<TItem> responseObject, ICacheStrategy strategy)
        {
            return responseObject.Count == 0 || strategy.Expired(responseObject);
        }
    }
}
