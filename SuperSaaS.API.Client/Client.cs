using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.Generic;
using SuperSaaS.API.Api;
using Newtonsoft.Json;

namespace SuperSaaS.API
{
    public class Client : IClient
    {
        public const string API_VERSION = "1";
        public const string VERSION = "0.9.0";
        public const int TIMEOUT_SECONDS = 10 * 1000;

        public string AccountName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Host { get; set; }
        public bool Test { get; set; }

        public Appointments Appointments { get; set; }
        public Forms Forms { get; set; }
        public Schedules Schedules { get; set; }
        public Users Users { get; set; }        

        HttpWebRequest LastRequest;

        public Client(Configuration configuration = null)
        {
            configuration = configuration ?? new Configuration();
            this.AccountName = configuration.AccountName;
            this.Password = configuration.Password;
            this.UserName = configuration.UserName;
            this.Host = configuration.Host;
            this.Test = configuration.Test;
            this.Init();
        }

        public Client(string accountName, string password, string userName = null, string host = null, bool test = false) 
        {
            this.AccountName = accountName;
            this.Password = password;
            this.UserName = userName;
            this.Host = host ?? Configuration.DEFAULT_HOST;
            this.Test = test;
            this.Init();
        }

        private void Init() {
            this.Appointments = new Appointments(this);
            this.Forms = new Forms(this);
            this.Schedules = new Schedules(this);
            this.Users = new Users(this);
        }

        public T Get<T>(string path, JsonArgs queryData = null)
        {
            return this.Request<T>(HttpMethod.GET, path, null, queryData);
        }
        public T Post<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null)
        {
            return this.Request<T>(HttpMethod.POST, path, postData);
        }
        public T Put<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null)
        {
            return this.Request<T>(HttpMethod.PUT, path, postData);
        }
        public T Delete<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null) {
            return this.Request<T>(HttpMethod.DELETE, path, postData);
        }

        private T Request<T>(string httpMethod, string path, NestedJsonArgs postData = null, JsonArgs queryData = null)
        {
            string url = this.Host + "/" + path + ".json" + this.dictionaryToQuerystring(queryData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = httpMethod;
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", this.basicAuth());
            request.UserAgent = this.userAgent();
            request.Timeout = TIMEOUT_SECONDS;

            string json = null;
            if (postData != null) {
                json = JsonConvert.SerializeObject(postData);
            }

            this.LastRequest = request;
            if (this.Test) {
                return default(T);
            }

            if (json != null) {
                using (Stream stream = request.GetRequestStream())
                {
                    using (StreamWriter streamOut = new StreamWriter(stream, System.Text.Encoding.ASCII))
                    {
                        streamOut.Write(json);
                    }
                }
            }

            string body = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (StreamReader streamIn = new StreamReader(stream))
                            {
                                body = streamIn.ReadToEnd();
                                streamIn.Close();
                            }
                            stream.Close();
                        }
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException && ((WebException)ex).Status == WebExceptionStatus.ProtocolError)
                {
                    using (WebResponse response = ((WebException)ex).Response)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (StreamReader r = new StreamReader(stream))
                                {
                                    body = r.ReadToEnd();
                                    r.Close();
                                }
                                stream.Close();
                            }
                        }
                        response.Close();
                    }
                    throw new SSSException(body);
                }
                else
                {
                    throw new SSSException(ex.Message);
                }
            }

            if (body.Length > 0)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                T result = JsonConvert.DeserializeObject<T>(body, settings);
                return result;
            } else {
                return default(T);
            }
        }

        private string basicAuth()
        {
            string auth = this.AccountName + ":" + this.Password;
            auth = Convert.ToBase64String(Encoding.Default.GetBytes(auth));
            return "Basic " + auth;
        }

        private string userAgent() {
            return "SSS/" + VERSION + " DOTNET/" + Environment.Version + " API/" + API_VERSION;
        }

        private string dictionaryToQuerystring(Dictionary<string, string> queryData)
        {
            if (queryData == null)
            {
                return "";
            }
            else
            {
                string query = "?";
                foreach (KeyValuePair<string, string> entry in queryData)
                {
                    query += entry.Key + "=" + System.Uri.EscapeDataString(entry.Value) + "&";
                }
                query = query.TrimEnd('&');
                return query;
            }
        }
    }
}
