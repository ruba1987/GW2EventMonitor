using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET
{
    /// <summary>
    /// Provides Exceptions for internal API.
    /// May be used to wrap other exceptions to provide specific problems with possible resolutions.
    /// </summary>
    public class GwApiException : Exception
    {
        /// <summary>
        /// Possible resolution to exception
        /// </summary>
        public string Suggestion { get; set; }
        /// <summary>
        /// Response Error from GW2
        /// </summary>
        public string ResponseError { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="innerException">inner exception</param>
        public GwApiException(string message, Exception innerException) :base(message, innerException)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="suggestion">Possible resolution to exception</param>
        /// <param name="innerException">inner exception</param>
        public GwApiException(string message, string suggestion = "", Exception innerException = null)
            : base(message, innerException)
        {
            Suggestion = suggestion;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="responseError">GW2 error resposne </param>
        public GwApiException(string message, string responseError = "")
            : base(message)
        {
            ResponseError = responseError;
        }
    }
}
