using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Parser for Render Services api.
    /// </summary>
    public class RenderServiceAssetParser : IApiResponseParserAsync<RenderServiceAssetEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RenderServiceAssetParser()
        {

        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>Parsed object</returns>
        public RenderServiceAssetEntry Parse(object apiResponse)
        {
            byte[] rawBytes = ParserResponseHelper.GetResponseRaw(apiResponse);
            var bitmap = new Bitmap(new MemoryStream(rawBytes));
            return new RenderServiceAssetEntry
                {
                    Asset = bitmap,
                };
        }

        public Task<RenderServiceAssetEntry> ParseAsync(object apiResponse)
        {
            byte[] rawBytes = ParserResponseHelper.GetResponseRaw(apiResponse);
            var tsk = new TaskCompletionSource<RenderServiceAssetEntry>();
            var bitmap = new Bitmap(new MemoryStream(rawBytes));
            tsk.TrySetResult(new RenderServiceAssetEntry
                {
                    Asset = bitmap,
                });
            return tsk.Task;
        }
    }
}
