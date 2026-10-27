using System.Collections.Generic;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture]
    public class UsersTest : TestModeFixture
    {
        [Test]
        public void GetTest()
        {
            Assert.DoesNotThrow(() => Client.Users.Get(123));
        }

        [Test]
        public void GetFkTest()
        {
            Assert.DoesNotThrow(() => Client.Users.Get("123fk"));
        }

        [Test]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => Client.Users.List(true, 10, 15));
        }

        [Test]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => Client.Users.Create(UserArgs()));
        }

        [Test]
        public void CreateFkTest()
        {
            Assert.DoesNotThrow(() => Client.Users.Create(UserArgs(), "123fk"));
        }

        [Test]
        public void UpdateTest()
        {
            Assert.DoesNotThrow(() => Client.Users.Update(123, UserArgs()));
        }

        [Test]
        public void DeleteTest()
        {
            Assert.DoesNotThrow(() => Client.Users.Delete(123));
        }

        private static Dictionary<string, string> UserArgs()
        {
            return new Dictionary<string, string>
            {
                { "name", "Test" },
                { "email", "test@example.com" },
                { "password", "pass123" },
                { "full_name", "Tester Test" },
                { "address", "123 St, City" },
                { "mobile", "555-5555" },
                { "phone", "555-5555" },
                { "country", "FR" },
                { "field_1", "f 1" },
                { "field_2", "f 2" },
                { "super_field", "sf" },
                { "credit", "10" },
                { "role", "3" }
            };
        }
    }
}
