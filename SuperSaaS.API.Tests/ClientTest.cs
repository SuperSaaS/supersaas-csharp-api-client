using NUnit.Framework;
using SuperSaaS.API.Api;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class ClientTest
    {
        public Client Client;

        [SetUp]
        public void Init()
        {
            Configuration config = new Configuration();
            config.AccountName = "accnt";
            config.Password = "pwd";
            config.Test = true;

            this.Client = new Client(config);
        }

        [Test()]
        public void ApiTest()
        {
            Assert.IsInstanceOf(typeof(Appointments), this.Client.Appointments);
        }

        [Test()]
        public void GetTest()
        {
            JsonArgs data = new JsonArgs { { "test", "true" } };
            Assert.AreEqual(default(object), this.Client.Get<object>("/test", data));
        }

        [Test()]
        public void PostTest()
        {
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "user", new JsonArgs { { "test", "true" } } }
            };
            Assert.AreEqual(default(object), this.Client.Post<object>("/test", data));
        }

        [Test()]
        public void PutTest()
        {
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "user", new JsonArgs { { "test", "true" } } }
            };
            Assert.AreEqual(default(object), this.Client.Put<object>("/test", data));
        }

        [Test()]
        public void DeleteTest()
        {
            JsonArgs data = new JsonArgs { { "id", "123" } };
            Assert.AreEqual(default(object), this.Client.Delete<object>("/test", null, data));
        }
    }
}
