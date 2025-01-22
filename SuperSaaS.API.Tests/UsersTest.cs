using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SuperSaaS.API.Tests
{
    [TestClass]
    public class UsersTest
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
        public void GetTest()
        {
            try
            {
                this.Client.Users.Delete(123);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }

            try
            {
                this.Client.Users.Get(123);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        [TestMethod]
        public void ListTest()
        {
            try
            {
                this.Client.Users.Delete(123);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }

            try
            {
                this.Client.Users.List(true, 10, 15);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            try
            {
                this.Client.Users.Delete(123);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }

            try
            {
                this.Client.Users.Create(this.UserArgs());
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            try
            {
                this.Client.Users.Update(123, this.UserArgs());
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            try
            {
                this.Client.Users.Delete(123);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        private Dictionary<string, object> UserArgs()
        {
            return new Dictionary<string, object> {
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
                {"credit", 10},
                {"role", 3}
            };
        }
    }
}
