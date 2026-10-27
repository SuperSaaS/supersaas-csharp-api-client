namespace SuperSaaS.API.Api
{
    /// <summary>Base class for the API endpoint groups; holds the client used to send requests.</summary>
    public class BaseApi
    {
        /// <summary>The client used to send requests.</summary>
        protected IClient Client;

        /// <summary>Creates an endpoint group that sends its requests through <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public BaseApi(IClient client)
        {
            Client = client;
        }

        /// <summary>
        /// Sends a POST request and returns the id of the created resource taken from the Location header, or 0.
        /// </summary>
        internal int PostAndGetResourceId<T>(string path, NestedJsonArgs data, JsonArgs query)
        {
            if (Client is global::SuperSaaS.API.Client client)
            {
                return client.PostAndGetResourceId<T>(path, data, query);
            }
            Client.Post<T>(path, data, query);
            return Client.GetResourceIdFromHeader();
        }
    }
}
