using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    /// <summary>
    /// Parser for the <seealso cref="Constants.files"/> api response
    /// </summary>
    public class FileEntryParser : IApiResponseParserAsync<EntryDictionary<string, FileEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FileEntryParser()
        {
        }

        /// <summary>
        /// Parses the response object.
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns></returns>
        public EntryDictionary<string, FileEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var files = ParserHelper<EntryDictionary<string, FileEntry>>.Parse(json);
            return files;
        }

        public async Task<EntryDictionary<string, FileEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var files = await ParserHelper<EntryDictionary<string, FileEntry>>.ParseAsync(json).ConfigureAwait(false);
            return files;
        }
    }
}
