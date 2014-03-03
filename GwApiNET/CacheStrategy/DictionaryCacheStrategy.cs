using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET.CacheStrategy
{

    public class DictionaryCacheStrategy<T, TKey, TItem> : ICacheStrategy where T : EntryDictionary<TKey, TItem> where TItem : ResponseObject
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DictionaryCacheStrategy()
        {
        }
        public bool Expired(ResponseObject responseObject)
        {

            var list = responseObject as T;
            if (list != null)
            {
                return list.Values.Count == 0 || Expired(list, list.Values.First().CacheStrategy);
            }
            return false;

        }

        public bool Expired(EntryDictionary<TKey, TItem> responseObject, ICacheStrategy strategy)
        {
            return responseObject.Values.Count == 0 || strategy.Expired(responseObject);
        }
    }
}
