using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry for render services api.
    /// </summary>
    [Serializable]
    public class RenderServiceAssetEntry : ResponseObject
    {
        /// <summary>
        /// Retrieved Asset
        /// </summary>
        public Bitmap Asset { get; set; }
        /// <summary>
        /// File information of the asset.  Used to retrieve the file.
        /// </summary>
        public FileEntry File { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RenderServiceAssetEntry()
        {

        }

        public override void SetResponse(object response)
        {
            RawResponse = response is IRestResponse
                           ? (response as IRestResponse).RawBytes
                           : response;
        }
    }
}
