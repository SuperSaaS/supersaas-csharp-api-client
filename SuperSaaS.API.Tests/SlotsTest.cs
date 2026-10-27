using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SuperSaaS.API.Models;
using NetHttpMethod = System.Net.Http.HttpMethod;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Checks the requests the slot endpoints build, through a fake message handler.
    /// </summary>
    [TestFixture]
    public class SlotsTest : FakeHttpFixture
    {
        [Test]
        public void ListRequestsSlotsOfTheSchedule()
        {
            Client.Slots.List(123);
            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Get));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/slots.json?schedule_id=123"));
        }

        [Test]
        public void ListPassesTimeRangeAndPagination()
        {
            Client.Slots.List(123, new DateTime(2030, 1, 1, 10, 0, 0), new DateTime(2030, 1, 8, 10, 0, 0), 10, 5);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/slots.json?schedule_id=123" +
                "&start=2030-01-01%2010%3A00%3A00&finish=2030-01-08%2010%3A00%3A00&limit=10&offset=5"));
        }

        [Test]
        public void ListDeserializesSlots()
        {
            RespondWith(HttpStatusCode.OK, "[{\"id\":1,\"title\":\"A\"},{\"id\":2,\"title\":\"B\"}]");

            Slot[] slots = Client.Slots.List(123);

            Assert.That(slots, Has.Length.EqualTo(2));
            Assert.That(slots[1].title, Is.EqualTo("B"));
        }

        [Test]
        public void GetRequestsTheSlot()
        {
            RespondWith(HttpStatusCode.OK, "{\"id\":456,\"title\":\"Test\",\"capacity\":2}");

            Slot slot = Client.Slots.Get(123, 456);

            Assert.That(LastUrl, Is.EqualTo(Host + "/api/slots/456.json?schedule_id=123"));
            Assert.That(slot.id, Is.EqualTo(456));
            Assert.That(slot.capacity, Is.EqualTo("2"));
        }

        [Test]
        public void DeserializesLevelAndWaiting()
        {
            // Shape of a GET /api/slots/:id.json response
            RespondWith(HttpStatusCode.OK, "{\"id\":306891795,\"start\":\"2026-10-24T14:20\",\"finish\":\"2026-10-24T15:18\"," +
                "\"title\":\"Reminder test\",\"capacity\":2,\"level\":1,\"waiting\":3,\"color\":0}");

            Slot slot = Client.Slots.Get(123, 306891795);

            Assert.That(slot.level, Is.EqualTo(1));
            Assert.That(slot.waiting, Is.EqualTo(3));
        }

        [Test]
        public void CreatePostsSlotAndReturnsIdFromLocationHeader()
        {
            RespondWith(HttpStatusCode.Created, "", Host + "/api/slots/789.json");

            Slot slot = Client.Slots.Create(123, SlotArgs());

            Assert.That(slot.id, Is.EqualTo(789));
            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Post));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/slots.json?schedule_id=123"));
            Assert.That(LastBody, Does.StartWith("{\"slot\":{\"start\":\"2030-01-01 10:00:00\","));
            Assert.That(LastBody, Does.Contain("\"capacity\":\"10\""));
        }

        [Test]
        public void CreateWithoutLocationHeaderReturnsIdZero()
        {
            Slot slot = Client.Slots.Create(123, SlotArgs());
            Assert.That(slot.id, Is.Zero);
        }

        [Test]
        public void UpdatePutsSlotAttributes()
        {
            Client.Slots.Update(123, 456, new Dictionary<string, string> { { "title", "Updated" } });

            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Put));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/slots/456.json?schedule_id=123"));
            Assert.That(LastBody, Is.EqualTo("{\"slot\":{\"title\":\"Updated\"}}"));
        }

        [Test]
        public void DeleteSendsDeleteWithoutBody()
        {
            Client.Slots.Delete(123, 456);

            Assert.That(LastMessage.Method, Is.EqualTo(NetHttpMethod.Delete));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/slots/456.json?schedule_id=123"));
            Assert.That(Handler.LastRequest.Body, Is.Null);
        }

        private static Dictionary<string, string> SlotArgs()
        {
            return new Dictionary<string, string>
            {
                { "start", "2030-01-01 10:00:00" },
                { "finish", "2030-01-01 11:00:00" },
                { "title", "Test" },
                { "description", "Testing" },
                { "location", "Room 1" },
                { "capacity", "10" },
                { "price", "5" },
                { "color", "#ff0000" }
            };
        }
    }
}
