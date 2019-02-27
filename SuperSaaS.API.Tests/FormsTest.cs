using System;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class FormsTest
    {
        public Client Client;

        [SetUp]
        public void Init()
        {
            Configuration config = new Configuration();
            config.AccountName = "accnt";
            config.ApiKey = "xxxxxxxxxxxxxxxxxxxx";
            config.Test = true;

            this.Client = new Client(config);
        }

        [Test()]
        public void GetTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Forms.Get(123); });
        }

        [Test()]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Forms.List(123, DateTime.Now); });
        }
    }
}
