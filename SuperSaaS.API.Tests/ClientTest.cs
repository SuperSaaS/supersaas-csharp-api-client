using NUnit.Framework;
using SuperSaaS.API.Api;

namespace SuperSaaS.API.Tests
{
    [TestFixture]
    public class ClientTest : TestModeFixture
    {
        [Test]
        public void ApiTest()
        {
            Assert.That(Client.Appointments, Is.InstanceOf<Appointments>());
        }

        [Test]
        public void GetTest()
        {
            var data = new JsonArgs { { "test", "true" } };
            Assert.That(Client.Get<object>("/test", data), Is.EqualTo(default(object)));
        }

        [Test]
        public void PostTest()
        {
            var data = new NestedJsonArgs
            {
                { "user", new JsonArgs { { "test", "true" } } }
            };
            Assert.That(Client.Post<object>("/test", data), Is.EqualTo(default(object)));
        }

        [Test]
        public void PutTest()
        {
            var data = new NestedJsonArgs
            {
                { "user", new JsonArgs { { "test", "true" } } }
            };
            Assert.That(Client.Put<object>("/test", data), Is.EqualTo(default(object)));
        }

        [Test]
        public void DeleteTest()
        {
            var data = new JsonArgs { { "id", "123" } };
            Assert.That(Client.Delete<object>("/test", null, data), Is.EqualTo(default(object)));
        }
    }
}
