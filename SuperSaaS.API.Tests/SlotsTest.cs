using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class SlotsTest
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
            Assert.DoesNotThrow(() => { this.Client.Slots.List(123); });
            Assert.DoesNotThrow(() => { this.Client.Slots.List(123, DateTime.Now, DateTime.Now.AddDays(7), 10, 5); });
        }

        [Test()]
        public void GetTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Slots.Get(123, 456); });
        }

        [Test()]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Slots.Create(123, this.SlotArgs()); });
        }

        [Test()]
        public void UpdateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Slots.Update(123, 456, this.SlotArgs()); });
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Slots.Delete(123, 456); });
        }

        private Dictionary<string, string> SlotArgs()
        {
            return new Dictionary<string, string> {
                {"start", "2030-01-01 10:00:00"},
                {"finish", "2030-01-01 11:00:00"},
                {"title", "Test"},
                {"description", "Testing"},
                {"location", "Room 1"},
                {"capacity", "10"},
                {"price", "5"},
                {"color", "#ff0000"}
            };
        }
    }
}
