using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class SchedulesTest
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
        public void ListTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Schedules.List(); });
        }

        [Test()]
        public void ResourcesTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Schedules.Resources(12345); });
        }
    }
}
