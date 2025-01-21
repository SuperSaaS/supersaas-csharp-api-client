using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSaaS.API.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SuperSaaS.API.Tests
{
    [TestClass]
    public class ClientTest
    {
        public Client Client;

        [TestInitialize]
        public void Init()
        {
            Configuration config = new Configuration
            {
                AccountName = "accnt",
                ApiKey = "xxxxxxxxxxxxxxxxxxxx",
                Test = true
            };

            this.Client = new Client(config);
        }

        [TestMethod]
        public void ApiTest()
        {
            Assert.IsInstanceOfType(this.Client.Appointments, typeof(Appointments));
        }

        [TestMethod]
        public void GetTest()
        {
            JsonArgs data = new JsonArgs { { "test", "true" } };
            Assert.AreEqual(default(object), this.Client.Get<object>("/test", data));
        }

        [TestMethod]
        public void PostTest()
        {
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "user", new JsonArgs { { "test", "true" } } }
            };
            Assert.AreEqual(default(object), this.Client.Post<object>("/test", data));
        }

        [TestMethod]
        public void PutTest()
        {
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "user", new JsonArgs { { "test", "true" } } }
            };
            Assert.AreEqual(default(object), this.Client.Put<object>("/test", data));
        }

        [TestMethod]
        public void DeleteTest()
        {
            JsonArgs data = new JsonArgs { { "id", "123" } };
            Assert.AreEqual(default(object), this.Client.Delete<object>("/test", new NestedJsonArgs(), data));
        }

        [TestMethod]
        public void TestRateLimit()
        {
            if (Environment.GetEnvironmentVariable("SSS_CSHARP_RATE_LIMITER_TEST") != "true") { return; }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 5; i++)
            {
                this.Client.Get<object>("/test", new JsonArgs());
            }

            stopwatch.Stop();
            double elapsedTime = stopwatch.Elapsed.TotalSeconds;

            Assert.IsTrue(elapsedTime >= 3.99,
                $"Rate limiting should cause 5 requests to take at least 4 seconds, but took {elapsedTime:0.00}s");
        }
    }
}
