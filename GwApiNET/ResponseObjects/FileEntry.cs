using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for files.json.
    /// File Entry used for render services.
    /// </summary>
    [Serializable]
    public class FileEntry : ResponseObject
    {
        // We are assuming FileID is an int, even though it is listed as a string on the wiki.
        // Let us know if we screwed this one up.
        /// <summary>
        /// File ID used to retrieve the file from GW2.
        /// </summary>
        [JsonProperty("file_id")]
        public int FileID { get; set; }
        /// <summary>
        /// File Signature used to retrieve the file from GW2.
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
