namespace GwApiNET
{
    /// <summary>
    /// Handles network resources.  Retrieves remote objects using network resources.
    /// </summary>
    public interface INetworkHandler
    {
        /// <summary>
        /// Get the api response for the given request
        /// </summary>
        /// <param name="request">api request</param>
        /// <returns>an api resource</returns>
        object GetResponse(IApiRequest request);

        /// <summary>
        /// Retrives the object for the given request.
        /// </summary>
        /// <typeparam name="T">Type of requested object</typeparam>
        /// <param name="request">API Request object used to build the resource url</param>
        /// <returns>requested object of type T</returns>
        T GetResponse<T>(IApiRequest request);
        /// <summary>
        /// The Base url to the api resources
        /// </summary>
        string BaseUrl { get; }

        string BuildUri(IApiRequest request);
    }
}
