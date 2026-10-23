using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SuperSaaS.API.Api;
using NetHttpMethod = System.Net.Http.HttpMethod;

namespace SuperSaaS.API
{
    /// <summary>
    /// Client for the SuperSaaS API. Holds the account credentials and exposes the API endpoints through
    /// <see cref="Appointments"/>, <see cref="Forms"/>, <see cref="Schedules"/>, <see cref="Slots"/> and
    /// <see cref="Users"/>.
    /// </summary>
    public class Client : IClient
    {
        /// <summary>Version of the SuperSaaS API this client talks to.</summary>
        public const string API_VERSION = "2";

        /// <summary>Version of this client library, sent in the User-Agent header.</summary>
        public const string VERSION = "1.1.0";

        /// <summary>
        /// Request timeout in <b>milliseconds</b> (10000 ms = 10 seconds). The name is kept for backwards
        /// compatibility.
        /// </summary>
        public const int TIMEOUT_SECONDS = 10 * 1000;

        private static readonly HttpClient SharedHttpClient = CreateSharedHttpClient();

        /// <summary>Timeout of the shared HTTP client: <see cref="TIMEOUT_SECONDS"/> milliseconds.</summary>
        internal static TimeSpan DefaultTimeout => SharedHttpClient.Timeout;

        private static readonly Regex ResourceIdPattern = new Regex(@"/(\d+)(\.json)?$", RegexOptions.CultureInvariant);

        private static readonly JsonSerializerSettings DeserializerSettings =
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        private string lastLocationHeader;

        /// <summary>The name of your SuperSaaS account.</summary>
        public string AccountName { get; set; }

        /// <summary>The API key of your SuperSaaS account, from the Account Info page.</summary>
        public string ApiKey { get; set; }

        /// <summary>Base URL of the SuperSaaS server, defaults to <see cref="Configuration.DEFAULT_HOST"/>.</summary>
        public string Host { get; set; }

        /// <summary>
        /// When <c>true</c>, requests are prepared but not sent and every call returns <c>default(T)</c>
        /// (<c>null</c> for models). Useful for testing code that uses the client.
        /// </summary>
        public bool Test { get; set; }

        /// <summary>Appointment and booking endpoints.</summary>
        public Appointments Appointments { get; set; }

        /// <summary>Form endpoints.</summary>
        public Forms Forms { get; set; }

        /// <summary>Schedule and resource endpoints.</summary>
        public Schedules Schedules { get; set; }

        /// <summary>Slot endpoints for capacity schedules.</summary>
        public Slots Slots { get; set; }

        /// <summary>User endpoints.</summary>
        public Users Users { get; set; }

        /// <summary>
        /// The HTTP client used to send requests. Defaults to one instance shared by all clients; tests replace it
        /// to intercept requests.
        /// </summary>
        internal HttpClient HttpClient { get; set; } = SharedHttpClient;

        /// <summary>
        /// Creates a client from a <see cref="Configuration"/>. Without a configuration the account name and API
        /// key are read from the <c>SSS_API_ACCOUNT_NAME</c> and <c>SSS_API_KEY</c> environment variables.
        /// </summary>
        /// <param name="configuration">Credentials and settings, or <c>null</c> for the defaults.</param>
        public Client(Configuration configuration = null)
        {
            configuration = configuration ?? new Configuration();
            AccountName = configuration.AccountName;
            ApiKey = configuration.ApiKey;
            Host = configuration.Host;
            Test = configuration.Test;
            Init();
        }

        /// <summary>Creates a client with explicit credentials.</summary>
        /// <param name="accountName">The name of your SuperSaaS account.</param>
        /// <param name="key">The API key of your SuperSaaS account.</param>
        /// <param name="host">Base URL of the SuperSaaS server, defaults to <see cref="Configuration.DEFAULT_HOST"/>.</param>
        /// <param name="test">When <c>true</c>, requests are not sent; see <see cref="Test"/>.</param>
        public Client(string accountName, string key, string host = null, bool test = false)
        {
            AccountName = accountName;
            ApiKey = key;
            Host = host ?? Configuration.DEFAULT_HOST;
            Test = test;
            Init();
        }

        private void Init()
        {
            Appointments = new Appointments(this);
            Forms = new Forms(this);
            Schedules = new Schedules(this);
            Slots = new Slots(this);
            Users = new Users(this);
        }

        /// <inheritdoc/>
        public T Get<T>(string path, JsonArgs queryData = null)
        {
            return Request<T>(HttpMethod.GET, path, null, queryData);
        }

        /// <inheritdoc/>
        public T Post<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null)
        {
            return Request<T>(HttpMethod.POST, path, postData, queryData);
        }

        /// <inheritdoc/>
        public T Put<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null)
        {
            return Request<T>(HttpMethod.PUT, path, postData, queryData);
        }

        /// <inheritdoc/>
        public T Delete<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null)
        {
            return Request<T>(HttpMethod.DELETE, path, postData, queryData);
        }

        /// <inheritdoc/>
        public int GetResourceIdFromHeader()
        {
            return ParseResourceId(lastLocationHeader);
        }

        /// <summary>
        /// Sends a POST request and returns the id of the created resource from the response's Location header,
        /// without going through state shared with other requests on this client.
        /// </summary>
        internal int PostAndGetResourceId<T>(string path, NestedJsonArgs postData, JsonArgs queryData)
        {
            Request<T>(HttpMethod.POST, path, postData, queryData, out string location);
            return ParseResourceId(location);
        }

        /// <summary>Extracts the numeric id at the end of a Location header; 0 when there is none.</summary>
        internal static int ParseResourceId(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return 0;
            }
            Match match = ResourceIdPattern.Match(location);
            return match.Success &&
                   int.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out int id)
                ? id
                : 0;
        }

        private T Request<T>(string httpMethod, string path, NestedJsonArgs postData, JsonArgs queryData)
        {
            return Request<T>(httpMethod, path, postData, queryData, out _);
        }

        private T Request<T>(string httpMethod, string path, NestedJsonArgs postData, JsonArgs queryData,
            out string location)
        {
            location = null;
            var uri = new Uri(Host + "/api" + path + ".json" + DictionaryToQuerystring(queryData));
            string json = postData != null ? JsonConvert.SerializeObject(postData) : null;

            if (Test)
            {
                return default(T);
            }

            string body;
            try
            {
                using (HttpRequestMessage request = BuildRequest(httpMethod, uri, json))
                using (HttpResponseMessage response = Send(request))
                {
                    body = ReadBody(response);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new SSSException(body);
                    }
                    location = response.Headers.Location?.OriginalString;
                }
            }
            catch (SSSException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SSSException(ex.Message, ex);
            }

            lastLocationHeader = location;
            return body.Length > 0 ? JsonConvert.DeserializeObject<T>(body, DeserializerSettings) : default(T);
        }

        private HttpRequestMessage BuildRequest(string httpMethod, Uri uri, string json)
        {
            var request = new HttpRequestMessage(new NetHttpMethod(httpMethod), uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuthCredentials());
            request.Headers.TryAddWithoutValidation("User-Agent", UserAgent());
            if (json != null)
            {
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return request;
        }

        private HttpResponseMessage Send(HttpRequestMessage request)
        {
#if NET5_0_OR_GREATER
            return HttpClient.Send(request);
#else
            return HttpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }

        private static string ReadBody(HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return "";
            }
#if NET5_0_OR_GREATER
            using (Stream stream = response.Content.ReadAsStream())
#else
            using (Stream stream = response.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult())
#endif
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private string BasicAuthCredentials()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(AccountName + ":" + ApiKey));
        }

        private static string UserAgent()
        {
            return "SSS/" + VERSION + " DOTNET/" + Environment.Version + " API/" + API_VERSION;
        }

        private static string DictionaryToQuerystring(Dictionary<string, string> queryData)
        {
            if (queryData == null || queryData.Count == 0)
            {
                return "";
            }
            return "?" + string.Join("&",
                queryData.Select(entry => Uri.EscapeDataString(entry.Key) + "=" + Uri.EscapeDataString(entry.Value)));
        }

        private static HttpClient CreateSharedHttpClient()
        {
#if NET5_0_OR_GREATER
            // Recycle pooled connections so DNS changes are picked up by the long-lived shared instance.
            var handler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(5) };
#else
            var handler = new HttpClientHandler();
#endif
            return new HttpClient(handler) { Timeout = TimeSpan.FromMilliseconds(TIMEOUT_SECONDS) };
        }
    }
}
