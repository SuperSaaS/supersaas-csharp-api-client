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

        [Test()]
        public void DeserializesLevelAndWaitingTest()
        {
            // Shape of a GET /api/slots/:id.json response
            string body = "{\"id\":306891795,\"start\":\"2026-10-24T14:20\",\"finish\":\"2026-10-24T15:18\",\"title\":\"Reminder test\",\"capacity\":2,\"level\":1,\"waiting\":3,\"color\":0}";
            var settings = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };
            var slot = Newtonsoft.Json.JsonConvert.DeserializeObject<SuperSaaS.API.Models.Slot>(body, settings);
            Assert.AreEqual(1, slot.level);
            Assert.AreEqual(3, slot.waiting);
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
