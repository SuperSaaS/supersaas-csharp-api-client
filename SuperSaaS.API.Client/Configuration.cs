using System;
namespace SuperSaaS.API
{
    public class Configuration
    {
        public const string DEFAULT_HOST = "https://www.supersaas.com";

        public string AccountName;
        public string Password;
        public string UserName;
        public string Host;
        public bool Test;

        public Configuration()
        {
            this.AccountName = Environment.GetEnvironmentVariable("SSS_SDK_ACCOUNT_NAME");
            this.Password = Environment.GetEnvironmentVariable("SSS_SDK_PASSWORD");
            this.UserName = Environment.GetEnvironmentVariable("SSS_SDK_USER_NAME");
            this.Test = false;
            this.Host = Configuration.DEFAULT_HOST;
        }
    }
}
