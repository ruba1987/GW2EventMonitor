using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects.Parsers;
using RestSharp;

namespace GwApiNET
{
    /// <summary>
    /// Exception helper
    /// </summary>
    internal static class ExceptionHelper
    {
        /// <summary>
        /// Throws exception if given object is null.
        /// </summary>
        /// <typeparam name="T">type of object to check</typeparam>
        /// <param name="obj">object to determine if null</param>
        /// <param name="objectName">name of object</param>
        internal static void ThrowOnNull<T>(T obj, string objectName)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(objectName, string.Format("{0} cannot be null", objectName));
        }

        public static ResponseException ResponseError(IRestResponse response, string message = "")
        {
            return ResponseError(response.Content);
        }

        public static ResponseException ResponseError(string response, string message = "")
        {
            var e = ParserHelper<InternalResponseException>.Parse(response);
            return new ResponseException(e, message);
        }

    }
}
