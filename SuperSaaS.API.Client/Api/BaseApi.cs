using System;

namespace SuperSaaS.API.Api
{
    public class BaseApi
    {
        protected IClient Client;

        public BaseApi(IClient client)
        {
            this.Client = client;
        }
    }
}
