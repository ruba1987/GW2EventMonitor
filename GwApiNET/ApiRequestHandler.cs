using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using RestSharp;

namespace GwApiNET
{

    public class ApiRequestHandler<T> : IApiRequestHandlerAsync<T> where T : ResponseObject
    {
        private readonly object _lock = new object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiRequestHandler(IApiResponseParser<T> parser, INetworkHandler networkHandler, bool ignoreCache = false)
            : this(parser as IApiResponseParserAsync<T>, parser, networkHandler, ignoreCache)
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiRequestHandler(IApiResponseParserAsync<T> parser, INetworkHandler networkHandler, bool ignoreCache = false)
            : this(parser, parser, networkHandler, ignoreCache)
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiRequestHandler(IApiResponseParserAsync<T> asyncParser, IApiResponseParser<T> parser, INetworkHandler networkHandler, bool ignoreCache = false)
        {
            ExceptionHelper.ThrowOnNull(parser, "parser");
            ExceptionHelper.ThrowOnNull(networkHandler, "networkHandler");
            Parser = parser;
            AsyncParser = asyncParser;
            IgnoreCache = ignoreCache;
            Network = networkHandler;
        }

        public IApiResponseParser<T> Parser { get; set; }

        public bool IgnoreCache { get; set; }

        /// <summary>
        /// request handler.  retrives a response using the IApiRequest
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>ResponseObject for the given request</returns>
        public T HandleRequest(IApiRequest request)
        {
            // We only want 1 API attempt at a time.
            // This will prevent two calls from retriving the same data at the same time.
            lock (_lock)
            {
                // First attempt to retrive the 
                ResponseObject response = ResponseCache.Cache.Get(Network.BuildUri(request), IgnoreCache);
                T apiResponse = null;

                if (response == null)
                {
                    Debug.WriteLine(string.Format("Getting {0} from GW2 API", typeof(T).Name));
                    GwApi.Logger.Debug("Getting {0} from GW2 API", typeof(T).Name);
                    // Response is not cached or has expired
                    var apiResponseObject = Network.GetResponse(request);
                    Debug.WriteLine(string.Format("Parsing {0}", request.Resource));
                    GwApi.Logger.Debug("Parsing {0}", request.Resource);
                    try
                    {
                        apiResponse = Parser.Parse(apiResponseObject);
                        if (apiResponse != null)
                        {
                            apiResponse.LastUpdated = DateTime.Now;
                            apiResponse.Url = Network.BuildUri(request);
                            apiResponse.SetResponse(apiResponseObject);
                            ResponseCache.Cache.Add(apiResponse);
                        }
                    }
                    catch (ResponseException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        GwApi.Logger.Error(e);
                        throw;
                    }
                }
                else
                {
                    Debug.WriteLine(string.Format("Getting {0} from cache", typeof(T).Name));
                    GwApi.Logger.Debug("Getting {0} from cache", typeof(T).Name);
                }
                //Debug.WriteLine(response == null ? "Retriving Response using " + Network.GetType() : "Using Cache");
                // Return cached response if it's not null
                // otherwise return the fresh new response
                return response as T ?? apiResponse;
            }
        }

        /// <summary>
        /// Network handler to use when handling the request.
        /// This is only used if IgnoreCache = true or the cache has no
        /// valid ResponseObject
        /// </summary>
        public INetworkHandler Network { get; set; }

        /// <summary>
        /// Asynchronous parser.
        /// </summary>
        public IApiResponseParserAsync<T> AsyncParser { get; set; }


        /// <summary>
        /// Asynchronous request handler
        /// </summary>
        /// <param name="request">api request</param>
        /// <returns>requested ResponseObject of type T</returns>
        public Task<T> HandleRequestAsync(IApiRequest request)
        {
            Task<T> task = null;
            return Task.Run(async () =>
                {
                    lock (_apiResponseTask)
                    {
                        if (_apiResponseTask.ContainsKey(Network.BuildUri(request)))
                        {
                            task = _apiResponseTask[Network.BuildUri(request)];
                        }
                        else
                        {
                            task = getResponse(request);
                            _apiResponseTask.TryAdd(Network.BuildUri(request), task);
                        }
                    }
                    //Debug.WriteLine("Awaiting task - {0} / {1}", task.Id, taskNum);
                    ResponseObject response = await task;
                    lock(_apiResponseTask)
                        _apiResponseTask.TryRemove(Network.BuildUri(request), out task);
                    return response as T;
                });
        }

        private async Task<T> getResponse(IApiRequest request)
        {
            // We only want 1 API attempt at a time.
            // This will prevent two calls from retriving the same data at the same time.
            //var task = Task.Run(async () =>
            //Debug.WriteLine("Obtaining response");
            // First attempt to retrive the response object
            ResponseObject
                response = ResponseCache.Cache.Get(Network.BuildUri(request), IgnoreCache);
            T apiResponse = null;

            if (response == null)
            {
                Debug.WriteLine(string.Format("Getting {0} from GW2 API - {1}", typeof(T).Name, Thread.CurrentContext.ContextID));
                GwApi.Logger.Debug("Getting {0} from GW2 API - {1}", typeof(T).Name, Thread.CurrentContext.ContextID);
                // Response is not cached or has expired
                var apiResponseObject = Network.GetResponse(request);
                Debug.WriteLine(string.Format("Parsing {0} - {1}", request.Resource, Thread.CurrentContext.ContextID));
                GwApi.Logger.Debug("Parsing {0} - {1}", request.Resource, Thread.CurrentContext.ContextID);
                try
                {
                    apiResponse = await AsyncParser.ParseAsync(apiResponseObject);
                    if (apiResponse != null)
                    {
                        apiResponse.LastUpdated = DateTime.Now;
                        apiResponse.Url = Network.BuildUri(request);
                        apiResponse.SetResponse(apiResponseObject);
                        ResponseCache.Cache.Add(apiResponse);
                    }
                }
                catch (ResponseException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("Getting {0} from GW2 API - {1}", typeof(T).Name, Thread.CurrentContext.ContextID));
                    GwApi.Logger.Error(e);
                    throw;
                }
            }
            else
            {
                Debug.WriteLine(string.Format("Getting {0} from cache - {1}", typeof(T).Name, Thread.CurrentContext.ContextID));
                GwApi.Logger.Debug("Getting {0} from cache - {1}", typeof(T).Name, Thread.CurrentContext.ContextID);
            }
            //Debug.WriteLine(response == null ? "Retriving Response using " + Network.GetType() : "Using Cache");
            // Return cached response if it's not null
            // otherwise return the fresh new response
            return response as T ?? apiResponse;
        }

        // used to track asynchronous api request handling 
        private static ConcurrentDictionary<string, Task<T>> _apiResponseTask = new ConcurrentDictionary<string, Task<T>>();
    }
}
