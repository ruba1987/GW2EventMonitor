using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET
{
    /// <summary>
    /// Cache for saving GW2 API responses.
    /// </summary>
    public class ResponseCache : IResponseCache, IResponseCacheAsync
    {
        private static ResponseCache _cache;
        /// <summary>
        /// Cache
        /// </summary>
        public static ResponseCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new ResponseCache();
                    if (File.Exists(Constants.CacheFile))
                        _cache.Load(Constants.CacheFile);
                }
                return _cache;
            }
        }

        Hashtable cache = new Hashtable();

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal ResponseCache()
        {
        }
        void test (){}
        /// <summary>
        /// Add an object to the response cache
        /// </summary>
        /// <param name="url">full url used to obtain the response</param>
        /// <param name="response">the response to cache</param>
        public void Add(string url, ResponseObject response)
        {
            response.Url = url;
            string hash = GetHash(url);
            lock (_objectLock)
            {
                if (cache.ContainsKey(hash))
                {
                    cache.Remove(hash);
                }
                response.Url = url;
                GwApi.Logger.Info("Adding {0} to Cache", response.GetType());
                cache.Add(hash, response);
            }
        }
        /// <summary>
        /// Add an object to the response cache
        /// </summary>
        /// <param name="response">the response to cache</param>
        public void Add(ResponseObject response)
        {
            Add(response.Url, response);
        }
        /// <summary>
        /// Retrieve a ResponseObject object from cache.
        /// </summary>
        /// <param name="url">full url that would be used to retrieve the response from the GW2 API</param>
        /// <param name="ignore">ignore any values.  This will force a return of null.</param>
        /// <returns>null is returned if no object is found, <paramref name="ignore"/> = true, or the object has expired; otherwise the requested response object for the given url will be returned.</returns>
        public ResponseObject Get(string url, bool ignore = false)
        {
            if(ignore) return null;
            string hash = GetHash(url);
            lock (_objectLock)
            {
                if (!cache.ContainsKey(hash)) return null;

                ResponseObject o = cache[hash] as ResponseObject;
                if (o != null && o.Expired)
                {
                    Debug.WriteLine(string.Format("{0} Expired Removing from cache", o.GetType().Name));
                    GwApi.Logger.Info("{0} Expired Removing from cache", o.GetType().Name);
                    
                    cache.Remove(hash);
                    return null;
                }
                else if (o != null)
                {
                    o.FromCache = true;
                    GwApi.Logger.Info("Retrieving {0} from cache ", o.GetType());
                    Debug.WriteLine(string.Format("Retrieving {0} from cache ", o.GetType().Name));
                }
                return o;
            }
        }

        public string[] gethashes()
        {
            string[] keys = new string[cache.Keys.Count];
            cache.Keys.CopyTo(keys, 0);
            return keys;
        }
        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear()
        {
            lock (_objectLock)
            {
                cache.Clear();
            }
        }
        /// <summary>
        /// Purge the cache of all expired objects.
        /// </summary>
        public void Purge()
        {
            lock (_objectLock)
            {
                foreach (ResponseObject obj in cache)
                {
                    if (obj.Expired) cache.Remove(GetHash(obj.Url));
                }
            }
        }
        /// <summary>
        /// Remove object from cache with given url
        /// </summary>
        /// <param name="url">url or key to object</param>
        public void Remove(string url)
        {
            string hash = GetHash(url);
            lock (_objectLock)
            {
                if (cache.ContainsKey(hash))
                {
                    cache.Remove(hash);
                }
            }
        }

        /// <summary>
        /// Obtain a hash value for a given string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHash(string value)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(value);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            sha1.ComputeHash(clearBytes);
            byte[] hashedBytes = sha1.Hash;
            sha1.Dispose();
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Saves the cache using the default cache file <seealso cref="Constants.CacheFile"/>
        /// </summary>
        public void Save()
        {
            Save(Constants.CacheFile);
        }
        /// <summary>
        /// Saves the cache file to the given filename.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            Task saveTask = SaveAsync(filename);
            saveTask.Wait();
        }

        /// <summary>
        /// Load a cache from the given file.
        /// if null is provided, then the <seealso cref="Constants.CacheFile"/> will attempt to be loaded.
        /// </summary>
        /// <param name="filename"></param>
        public void Load(string filename = null)
        {
            Task loadTask = LoadAsync(filename);
            loadTask.Wait();
        }


        private readonly Dictionary<string, object> _fileLocks = new Dictionary<string, object>();
        private readonly object _objectLock = new object();

        public Task AddAsync(string key, ResponseObject response)
        {
            return Task.Run(() =>
                {
                    response.Url = key;
                    string hash = GetHash(key);
                    lock (_objectLock)
                    {
                        if (cache.ContainsKey(hash))
                        {
                            cache.Remove(hash);
                        }
                        response.Url = key;
                        GwApi.Logger.Info("Adding {0} to Cache", response.GetType());
                        cache.Add(hash, response);
                    }
                });
        }

        public Task AddAsync(ResponseObject response)
        {
            return AddAsync(response.Url, response);
        }

        public Task<ResponseObject> GetAsync(string url, bool ignore = false)
        {
            return Task.Run(() =>
                {
                    if (ignore) return null;
                    string hash = GetHash(url);
                    lock (_objectLock)
                    {
                        if (!cache.ContainsKey(hash)) return null;

                        ResponseObject o = cache[hash] as ResponseObject;
                        if (o != null && o.Expired)
                        {
                            cache.Remove(hash);
                            o = null;
                        }
                        else if (o != null)
                        {
                            o.FromCache = true;
                            GwApi.Logger.Info("Retrieving {0} from cache ", o.GetType());
                        }
                        return o;
                    }
                });
        }

        public Task PurgeAsync()
        {
            return Task.Run(() =>
                {
                    lock (_objectLock)
                    {
                        foreach (ResponseObject obj in cache)
                        {
                            if (obj.Expired) cache.Remove(GetHash(obj.Url));
                        }
                    }
                });
        }

        public Task RemoveAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            return SaveAsync(Constants.CacheFile);
        }

        public Task SaveAsync(string filename)
        {
            Task task = _saveAsync(filename);
            return task;
        }

        public Task LoadAsync(string filename = null)
        {
            filename = filename ?? Constants.CacheFile;
            Task task = _loadAsync(filename);
            return task;
        }

        private object getFileLock(string filename)
        {
            lock (_fileLocks)
            {
                if (_fileLocks.ContainsKey(filename) == false)
                    _fileLocks[filename] = new object();
                return _fileLocks[filename];
            }
        }
        Task _saveAsync(string filename)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        byte[] binaryCache;
                        GwApi.Logger.Info("Saving Cache to - {0}", filename);
                        lock (_objectLock)
                        {
                            binaryCache = BinarySerializer.BinarySerialize(cache);
                        }
                        using (MemoryStream mstream = new MemoryStream())
                        {
                            using (GZipStream gStream = new GZipStream(mstream, CompressionLevel.Optimal))
                            {
                                gStream.Write(binaryCache, 0, binaryCache.Length);
                                gStream.Flush();
                            }
                            lock (getFileLock(filename)) // Write cache to file.
                                File.WriteAllBytes(filename, mstream.ToArray());
                        }
                    }
                    catch (Exception e)
                    {
                        string datetime = DateTime.Now.ToString(@"HH\_mm\_ss");
                        File.WriteAllText("Error_" + datetime + ".xml", "Error saving cache\n" + e.Message);
                        GwApi.Logger.Error(e);
                    }
                });
        }

        private readonly ConcurrentBag<Task> fileTasks = new ConcurrentBag<Task>();
        Task _loadAsync(string filename)
        {
            return Task.Run(() =>
                {
                    if (File.Exists(filename))
                    {
                        byte[] data;
                        lock (getFileLock(filename))
                            data = File.ReadAllBytes(filename);
                        using (MemoryStream stream = new MemoryStream(data))
                        {
                            using (GZipStream gStream = new GZipStream(stream, CompressionMode.Decompress))
                            {
                                GwApi.Logger.Info("Loading Cache from - {0}", filename);
                                lock (_objectLock)
                                {
                                    try
                                    {
                                        cache = BinarySerializer.BinaryDeserialize<Hashtable>(gStream);
                                        File.WriteAllLines("Keys.txt", Cache.gethashes());
                                    }
                                    catch (Exception e)
                                    {
                                        GwApi.Logger.Error(e, "Error loading cache");
                                    }
                                }
                            }
                        }
                    }
                });
        }
    }
}
