using System;
using System.Collections.Generic;

namespace SuperSaaS.API
{
    public interface IClient
    {
        T Get<T>(string path, JsonArgs queryData);
        T Post<T>(string path, NestedJsonArgs postData, JsonArgs queryData);
        T Put<T>(string path, NestedJsonArgs postData, JsonArgs queryData);
        T Delete<T>(string path, NestedJsonArgs postData, JsonArgs queryData);
        int GetResourceIdFromHeader();
    }
}
