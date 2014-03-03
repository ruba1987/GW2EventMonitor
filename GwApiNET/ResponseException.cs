using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET
{
    // TODO : finished response exception
    /// <summary>
    /// Response error for GW2 responses
    /// </summary>
    public class ResponseException : GwApiException
    {
        internal string CustomMessage { get; set; }
        [JsonProperty("error")]
        public int Error { get; private set; }
        [JsonProperty("product")]
        public int Product { get; private set; }
        [JsonProperty("module")]
        public int Module { get; private set; }
        [JsonProperty("line")]
        public int Line { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
        public override string Message
        {
            get
            {
                return (base.Message ?? "") + (CustomMessage ?? "") + (Text ?? "");
            }
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        [JsonConstructor()]
        public ResponseException() : base("", ""){}
        //public ResponseException(string message)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResponseException(string message)
            : base(message, "")
        {
        }

        internal ResponseException(InternalResponseException exception, string message) : this(message)
        {
            Error = exception.Error;
            Product = exception.Product;
            Module = exception.Module;
            Line = exception.Line;
            Text = exception.Text;
        }
    }

    internal class InternalResponseException
    {
        [JsonProperty("error")]
        public int Error { get; private set; }
        [JsonProperty("product")]
        public int Product { get; private set; }
        [JsonProperty("module")]
        public int Module { get; private set; }
        [JsonProperty("line")]
        public int Line { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }

        public InternalResponseException()
        {}
    }
}
