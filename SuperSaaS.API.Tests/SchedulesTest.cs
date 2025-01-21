using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SuperSaaS.API.Tests
{
    [TestClass]
    public class SchedulesTest
    {
        public Client Client;

        [TestInitialize]
        public void Init()
        {
            Configuration config = new Configuration();
            config.AccountName = "accnt";
            config.ApiKey = "xxxxxxxxxxxxxxxxxxxx";
            config.Test = true;

            this.Client = new Client(config);
        }

        [TestMethod]
        public void ListTest()
        {
            try
            {
                this.Client.Schedules.List();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        [TestMethod]
        public void ResourcesTest()
        {
            try
            {
                this.Client.Schedules.Resources(12345);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }
    }
}
