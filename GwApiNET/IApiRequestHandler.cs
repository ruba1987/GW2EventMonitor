using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using RestSharp;

namespace GwApiNET
{
    /// <summary>
    /// Interface definition to handle api requests.
    /// </summary>
    /// <typeparam name="T">Expected object that will be parsed</typeparam>
    public interface IApiRequestHandler<T> where T : ResponseObject
    {
        /// <summary>
        /// api parser
        /// </summary>
        IApiResponseParser<T> Parser { get; set; }
        /// <summary>
        /// ignore cache forces an updated GW2 response
        /// </summary>
        bool IgnoreCache { get; set; }
        /// <summary>
        /// request handler.  retrives a response using the IApiRequest
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>ResponseObject for the given request</returns>
        T HandleRequest(IApiRequest request);
        /// <summary>
        /// Network handler to use when handling the request.
        /// This is only used if IgnoreCache = true or the cache has no
        /// valid ResponseObject
        /// </summary>
        INetworkHandler Network { get; set; }
    }

    /// <summary>
    /// Interface definition to handle api requests.
    /// </summary>
    /// <typeparam name="T">Expected object that will be parsed</typeparam>
    public interface IApiRequestHandlerAsync<T> : IApiRequestHandler<T> where T : ResponseObject
    {
        /// <summary>
        /// Asynchronous parser.
        /// </summary>
        IApiResponseParserAsync<T> AsyncParser { get; set; }
        /// <summary>
        /// Asynchronous request handler
        /// </summary>
        /// <param name="request">api request</param>
        /// <returns>requested ResponseObject of type T</returns>
        Task<T> HandleRequestAsync(IApiRequest request);
    }
    /// <summary>
    /// Asynchronous parser interface
    /// </summary>
    /// <typeparam name="T">ResponseObject type</typeparam>
    public interface IApiResponseParserAsync<T> : IApiResponseParser<T>
    {
        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        Task<T> ParseAsync(object apiResponse);
    }

    /// <summary>
    /// Response Parser Interface definition.
    /// <remarks>Each GW2 API response will likely have it's own IApiResponseParser{T}</remarks>
    /// </summary>
    /// <typeparam name="T">Expected output object</typeparam>
    public interface IApiResponseParser<out T>
    {
        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        T Parse(object apiResponse);
    }
}
