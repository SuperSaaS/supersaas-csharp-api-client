namespace SuperSaaS.API
{
    /// <summary>HTTP method names used by the client.</summary>
    public static class HttpMethod
    {
        /// <summary>HTTP GET.</summary>
        public const string GET = "GET";

        /// <summary>HTTP POST.</summary>
        public const string POST = "POST";

        /// <summary>HTTP PUT.</summary>
        public const string PUT = "PUT";

        /// <summary>HTTP DELETE.</summary>
        public const string DELETE = "DELETE";
    }

    /// <summary>
    /// Sends requests to the SuperSaaS API. Paths are relative to <c>/api</c> and get a <c>.json</c> suffix,
    /// e.g. <c>"/users"</c> requests <c>{Host}/api/users.json</c>.
    /// </summary>
    public interface IClient
    {
        /// <summary>Sends a GET request.</summary>
        /// <typeparam name="T">Type the JSON response is deserialized into.</typeparam>
        /// <param name="path">API path, e.g. <c>"/users"</c>.</param>
        /// <param name="queryData">Query string parameters.</param>
        /// <returns>The deserialized response, or <c>default(T)</c> for an empty response or in test mode.</returns>
        /// <exception cref="SSSException">
        /// The API returned an error (the message is the response body) or the request failed.
        /// </exception>
        T Get<T>(string path, JsonArgs queryData = null);

        /// <summary>Sends a POST request with a JSON body.</summary>
        /// <typeparam name="T">Type the JSON response is deserialized into.</typeparam>
        /// <param name="path">API path, e.g. <c>"/users"</c>.</param>
        /// <param name="postData">Request body, serialized as JSON.</param>
        /// <param name="queryData">Query string parameters.</param>
        /// <returns>The deserialized response, or <c>default(T)</c> for an empty response or in test mode.</returns>
        /// <exception cref="SSSException">
        /// The API returned an error (the message is the response body) or the request failed.
        /// </exception>
        T Post<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null);

        /// <summary>Sends a PUT request with a JSON body.</summary>
        /// <typeparam name="T">Type the JSON response is deserialized into.</typeparam>
        /// <param name="path">API path, e.g. <c>"/users/123"</c>.</param>
        /// <param name="postData">Request body, serialized as JSON.</param>
        /// <param name="queryData">Query string parameters.</param>
        /// <returns>The deserialized response, or <c>default(T)</c> for an empty response or in test mode.</returns>
        /// <exception cref="SSSException">
        /// The API returned an error (the message is the response body) or the request failed.
        /// </exception>
        T Put<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null);

        /// <summary>Sends a DELETE request.</summary>
        /// <typeparam name="T">Type the JSON response is deserialized into.</typeparam>
        /// <param name="path">API path, e.g. <c>"/users/123"</c>.</param>
        /// <param name="postData">Optional request body, serialized as JSON.</param>
        /// <param name="queryData">Query string parameters.</param>
        /// <returns>The deserialized response, or <c>default(T)</c> for an empty response or in test mode.</returns>
        /// <exception cref="SSSException">
        /// The API returned an error (the message is the response body) or the request failed.
        /// </exception>
        T Delete<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null);

        /// <summary>
        /// Returns the id of the resource created by the last successful request, taken from the Location header
        /// of its response; 0 when there was no Location header or it did not end in a numeric id.
        /// </summary>
        /// <returns>The id of the created resource, or 0.</returns>
        int GetResourceIdFromHeader();
    }
}
