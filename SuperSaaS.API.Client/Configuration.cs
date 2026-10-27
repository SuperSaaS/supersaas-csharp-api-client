using System;

namespace SuperSaaS.API
{
    /// <summary>
    /// Settings for a <see cref="Client"/>. A new configuration reads the account name and API key from the
    /// <c>SSS_API_ACCOUNT_NAME</c> and <c>SSS_API_KEY</c> environment variables.
    /// </summary>
    public class Configuration
    {
        /// <summary>The default SuperSaaS server.</summary>
        public const string DEFAULT_HOST = "https://www.supersaas.com";

        /// <summary>The name of your SuperSaaS account.</summary>
        public string AccountName;

        /// <summary>The API key of your SuperSaaS account, from the Account Info page.</summary>
        public string ApiKey;

        /// <summary>Base URL of the SuperSaaS server.</summary>
        public string Host;

        /// <summary>When <c>true</c>, the client does not send requests; see <see cref="Client.Test"/>.</summary>
        public bool Test;

        /// <summary>
        /// Creates a configuration with credentials from the <c>SSS_API_ACCOUNT_NAME</c> and <c>SSS_API_KEY</c>
        /// environment variables and the default host.
        /// </summary>
        public Configuration()
        {
            AccountName = Environment.GetEnvironmentVariable("SSS_API_ACCOUNT_NAME");
            ApiKey = Environment.GetEnvironmentVariable("SSS_API_KEY");
            Test = false;
            Host = DEFAULT_HOST;
        }
    }
}
