using NUnit.Framework;
using System.Collections.Generic;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class UsersTest
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
            Assert.DoesNotThrow(() => { this.Client.Users.Get(123); });
        }

        [Test()]
        public void GetFkTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Users.Get("123fk"); });
        }

        [Test()]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Users.List(true, 10, 15); });
        }

        [Test()]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Users.Create(this.UserArgs()); });
        }

        [Test()]
        public void CreateFkTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Users.Create(this.UserArgs(), "123fk"); });
        }

        [Test()]
        public void UpdateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Users.Update(123, this.UserArgs()); });
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Users.Delete(123); });
        }

        private Dictionary<string, string> UserArgs() {
            return new Dictionary<string, string> {
                {"name", "Test"},
                {"email", "test@example.com"},
                {"password", "pass123"},
                {"full_name", "Tester Test"},
                {"address", "123 St, City"},
                {"mobile", "555-5555"},
                {"phone", "555-5555"},
                {"country", "FR"},
                {"field_1", "f 1"},
                {"field_2", "f 2"},
                {"super_field", "sf"},
                {"credit", "10"},
                {"role", "3"}
            };
        }
    }
}
