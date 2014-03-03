using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET
{

    public interface IResponseCache
    {
        /// <summary>
        /// Add an object to the response cache
        /// </summary>
        /// <param name="url">full url used to obtain the response</param>
        /// <param name="response">the response to cache</param>
        void Add(string key, ResponseObject response);
        /// <summary>
        /// Add an object to the response cache
        /// </summary>
        /// <param name="response">the response to cache</param>
        void Add(ResponseObject response);
        /// <summary>
        /// Retrieve a ResponseObject object from cache.
        /// </summary>
        /// <param name="url">full url that would be used to retrieve the response from the GW2 API</param>
        /// <param name="ignore">ignore any values.  This will force a return of null.</param>
        /// <returns>null is returned if no object is found, <paramref name="ignore"/> = true, or the object has expired; otherwise the requested response object for the given url will be returned.</returns>
        ResponseObject Get(string url, bool ignore = false);
        /// <summary>
        /// Clears the cache
        /// </summary>
        void Clear();
        /// <summary>
        /// Purge the cache of all expired objects.
        /// </summary>
        void Purge();
        /// <summary>
        /// Remove object from cache with given url
        /// </summary>
        /// <param name="url">url or key to object</param>
        void Remove(string url);
        /// <summary>
        /// Saves the cache using the default cache file <seealso cref="Constants.CacheFile"/>
        /// </summary>
        void Save();
        /// <summary>
        /// Saves the cache file to the given filename.
        /// </summary>
        /// <param name="filename"></param>
        void Save(string filename);
        /// <summary>
        /// Load a cache from the given file.
        /// if null is provided, then the <seealso cref="Constants.CacheFile"/> will attempt to be loaded.
        /// </summary>
        /// <param name="filename"></param>
        void Load(string filename = null);
    }

    public interface IResponseCacheAsync
    {
        /// <summary>
        /// Add an object to the response cache
        /// </summary>
        /// <param name="url">full url used to obtain the response</param>
        /// <param name="response">the response to cache</param>
        Task AddAsync(string key, ResponseObject response);
        /// <summary>
        /// Add an object to the response cache
        /// </summary>
        /// <param name="response">the response to cache</param>
        Task AddAsync(ResponseObject response);
        /// <summary>
        /// Retrieve a ResponseObject object from cache.
        /// </summary>
        /// <param name="url">full url that would be used to retrieve the response from the GW2 API</param>
        /// <param name="ignore">ignore any values.  This will force a return of null.</param>
        /// <returns>null is returned if no object is found, <paramref name="ignore"/> = true, or the object has expired; otherwise the requested response object for the given url will be returned.</returns>
        Task<ResponseObject> GetAsync(string url, bool ignore = false);
        /// <summary>
        /// Purge the cache of all expired objects.
        /// </summary>
        Task PurgeAsync();
        /// <summary>
        /// Remove object from cache with given url
        /// </summary>
        /// <param name="url">url or key to object</param>
        Task RemoveAsync(string url);
        /// <summary>
        /// Saves the cache using the default cache file <seealso cref="Constants.CacheFile"/>
        /// </summary>
        Task SaveAsync();
        /// <summary>
        /// Saves the cache file to the given filename.
        /// </summary>
        /// <param name="filename"></param>
        Task SaveAsync(string filename);
        /// <summary>
        /// Load a cache from the given file.
        /// if null is provided, then the <seealso cref="Constants.CacheFile"/> will attempt to be loaded.
        /// </summary>
        /// <param name="filename"></param>
        Task LoadAsync(string filename = null);
    }
}
