using System;
namespace SuperSaaS.API
{
    public class Configuration
    {
        public const string DEFAULT_HOST = "https://www.supersaas.com";

        public string AccountName;
        public string ApiKey;
        public string Host;
        public bool Test;

        public Configuration()
        {
            this.AccountName = Environment.GetEnvironmentVariable("SSS_API_ACCOUNT_NAME");
            this.ApiKey = Environment.GetEnvironmentVariable("SSS_API_KEY");
            this.Test = false;
            this.Host = Configuration.DEFAULT_HOST;
        }
    }
}
