using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using SuperSaaS.API.Api;

namespace SuperSaaS.API
{
    public class Client : IClient
    {
        public const string API_VERSION = "3";
        public const string VERSION = "2.0.0";
        public const int TIMEOUT_SECONDS = 10;

        private static readonly HttpClient _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(TIMEOUT_SECONDS)
        };

        private DateTime _lastRequestTime = DateTime.MinValue;
        private readonly object _lockObject = new();
        private readonly TimeSpan _minRequestInterval = TimeSpan.FromSeconds(1);

        public string? AccountName { get; set; }
        public string? ApiKey { get; set; }
        public string? Host { get; set; }
        public bool? Test { get; set; }

        public Appointments Appointments { get; set; } = null!;
        public Forms Forms { get; set; } = null!;
        public Schedules Schedules { get; set; } = null!;
        public Users Users { get; set; } = null!;
        public Groups Groups { get; set; } = null!;
        public Promotions Promotions { get; set; } = null!;

        private string? LocationHeader;

        public Client(Configuration? configuration = null)
        {
            configuration ??= new Configuration();
            AccountName = configuration.AccountName;
            ApiKey = configuration.ApiKey;
            Host = configuration.Host;
            Test = configuration.Test;
            Init();
        }

        public Client(string accountName, string key, string? host = null, bool test = false)
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
            Users = new Users(this);
            Promotions = new Promotions(this);
            Groups = new Groups(this);
        }

        public T Get<T>(string path, JsonArgs? queryData = null)
        {
            return Request<T>(HttpMethod.Get, path, null, queryData);
        }

        public T Post<T>(string path, NestedJsonArgs? postData = null, JsonArgs? queryData = null)
        {
            return Request<T>(HttpMethod.Post, path, postData, queryData);
        }

        public T Put<T>(string path, NestedJsonArgs? postData = null, JsonArgs? queryData = null)
        {
            return Request<T>(HttpMethod.Put, path, postData, queryData);
        }

        public T Delete<T>(string path, NestedJsonArgs? postData = null, JsonArgs? queryData = null)
        {
            return Request<T>(HttpMethod.Delete, path, postData, queryData);
        }

        public int GetResourceIdFromHeader()
        {
            if (string.IsNullOrEmpty(LocationHeader))
                return 0;

            var match = Regex.Match(LocationHeader, @"/(\d*?)(\.json)?$");
            return int.Parse(match.Groups[1].Value);
        }

        private void ApplyRateLimit()
        {
            lock (_lockObject)
            {
                var waitTime = _minRequestInterval - (DateTime.Now - _lastRequestTime);
                if (waitTime > TimeSpan.Zero)
                {
                    Thread.Sleep(waitTime);
                }
                _lastRequestTime = DateTime.Now;
            }
        }

        private T Request<T>(
            HttpMethod method,
            string path,
            NestedJsonArgs? postData = null,
            JsonArgs? queryData = null)
        {
            ApplyRateLimit();

            if (string.IsNullOrEmpty(AccountName))
                throw new SSSException("Account name not configured. Call `Client.Configure`.");
            if (string.IsNullOrEmpty(ApiKey))
                throw new SSSException("Account API key not configured. Call `Client.Configure`.");

            if (queryData != null)
                queryData.RemoveEmptyValues();
            if (postData != null)
                postData.RemoveEmptyValues();

            var apiPath = $"/api{path}.json";
            if (queryData?.Any() == true)
            {
                apiPath += "?" + DictionaryToQuerystring(queryData);
            }

            var url = $"{Host}{apiPath}";
            using var request = new HttpRequestMessage(method, url);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuth());
            request.Headers.UserAgent.ParseAdd(UserAgent());

            if (method != HttpMethod.Get && postData != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(postData),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            if (Test == true)
                return default!;

            try
            {
                var sendTask = _httpClient.SendAsync(request);
                sendTask.Wait();
                var response = sendTask.Result;

                var bodyTask = response.Content.ReadAsStringAsync();
                bodyTask.Wait();
                var body = bodyTask.Result;

                if (!response.IsSuccessStatusCode)
                    throw new SSSException(body);

                LocationHeader = response.Headers.Location?.ToString();
                Console.WriteLine(LocationHeader);

                if (string.IsNullOrEmpty(body))
                {
                    if (typeof(T) == typeof(string))
                    {

                        Console.WriteLine("23454345kkkkkk");
                        Console.WriteLine((T)(object)(LocationHeader ?? string.Empty));

                        return (T)(object)(LocationHeader ?? string.Empty);
                    }
                    return default!;
                }

                return JsonConvert.DeserializeObject<T>(
                    body,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })!;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is HttpRequestException httpEx)
                {
                    throw new SSSException($"HTTP Request Error: {httpEx.Message}");
                }
                throw new SSSException($"An error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private string BasicAuth()
        {
            var auth = $"{AccountName}:{ApiKey}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
        }

        private string UserAgent()
        {
            return $"SSS/{VERSION} DOTNET/{Environment.Version} API/{API_VERSION}";
        }

        private static string DictionaryToQuerystring(Dictionary<string, string>? queryData)
        {
            if (queryData?.Any() != true)
                return string.Empty;

            var query = string.Join("&",
                queryData.Select(kvp =>
                    $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            return $"?{query}";
        }
    }
 
    public static class DictionaryExtensions
    {
        public static void RemoveEmptyValues(this JsonArgs dict)
        {
            if (dict == null) return;
            
            var emptyKeys = dict.Keys.Where(key => 
                string.IsNullOrEmpty(dict[key])).ToList();
                
            foreach (var key in emptyKeys)
            {
                dict.Remove(key);
            }
        }

        public static void RemoveEmptyValues(this NestedJsonArgs dict)
        {
            if (dict == null) return;
            
            var emptyKeys = dict.Keys.Where(key => 
                dict[key] == null || string.IsNullOrEmpty(dict[key].ToString())).ToList();
                
            foreach (var key in emptyKeys)
            {
                dict.Remove(key);
            }
        }
    }
}

