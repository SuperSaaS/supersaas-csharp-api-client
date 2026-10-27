using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Base fixture providing a client in test mode: requests are built but never sent.
    /// </summary>
    public abstract class TestModeFixture
    {
        protected Client Client { get; private set; } = null!;

        [SetUp]
        public void CreateClient()
        {
            var config = new Configuration
            {
                AccountName = "accnt",
                ApiKey = "xxxxxxxxxxxxxxxxxxxx",
                Test = true
            };
            Client = new Client(config);
        }
    }
}
