using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET
{
    /// <summary>
    /// Represents a resource request and builds the appropriate url for use with the <seealso cref="INetworkHandler"/> 
    /// </summary>
    public interface IApiRequest 
    {
        /// <summary>
        /// Parameter List
        /// key = parameter name
        /// object = parameter value
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }
        /// <summary>
        /// Resource url (not including the base url)
        /// </summary>
        string Resource { get; set; }
        /// <summary>
        /// Add a parameter.
        /// </summary>
        /// <param name="name">name of parameter</param>
        /// <param name="value">value of parameter</param>
        void AddParameter(string name, object value);
    }
}
