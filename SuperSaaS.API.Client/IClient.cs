using System;
using System.Collections.Generic;

namespace SuperSaaS.API
{
    public static class HttpMethod
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
    }

    public interface IClient
    {
        T Get<T>(string path, JsonArgs queryData = null);
        T Post<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null);
        T Put<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null);
        T Delete<T>(string path, NestedJsonArgs postData = null, JsonArgs queryData = null);
        int GetResourceIdFromHeader();
    }
}
