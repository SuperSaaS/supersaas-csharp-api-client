using System;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    /// <summary>Parsing of the appointment responses, with bodies as the API sends them.</summary>
    [TestFixture]
    public class AppointmentResponseTest : FakeHttpFixture
    {
        private const string Booking =
            "{\"id\":101,\"schedule_id\":345,\"user_id\":7,\"resource_id\":611,\"res_name\":\"Meeting Room 1\"," +
            "\"start\":\"2026-11-02T09:00\",\"finish\":\"2026-11-02T10:00\",\"full_name\":\"Tester Test\"," +
            "\"created_on\":\"2026-10-27T08:15:00Z\",\"created_by\":\"administrator\",\"deleted\":false}";

        private const string UpdatedBooking =
            "{\"id\":102,\"schedule_id\":345,\"user_id\":7,\"resource_id\":611,\"res_name\":\"Meeting Room 1\"," +
            "\"start\":\"2026-11-02T11:00\",\"finish\":\"2026-11-02T12:00\",\"full_name\":\"Tester Test\"," +
            "\"created_on\":\"2026-10-27T08:15:00Z\",\"created_by\":\"administrator\"," +
            "\"deleted\":false}";

        private const string FreeSlot =
            "{\"id\":611531309,\"name\":\"Meeting Room 1\",\"count\":1,\"start\":\"2026-11-02T09:00\"," +
            "\"finish\":\"2026-11-02T13:00\"}";

        private const string CapacitySlot =
            "{\"id\":149631371,\"start\":\"2035-01-10T14:00\",\"finish\":\"2035-01-10T18:00\",\"title\":\"Rem test\"," +
            "\"description\":\"\",\"location\":\"\",\"capacity\":2,\"count\":1,\"bookings\":[" + Booking + "]}";

        [Test]
        public void AgendaReadsTheBookingsObject()
        {
            RespondWith(HttpStatusCode.OK, "{\"bookings\":[" + Booking + "," + UpdatedBooking + "]}");

            Appointment[] agenda = Client.Appointments.Agenda(345, 7, DateTime.MinValue);

            Assert.That(agenda.Select(a => a.id), Is.EqualTo(new[] { 101, 102 }));
            Assert.That(agenda[0].res_name, Is.EqualTo("Meeting Room 1"));
        }

        [Test]
        public void AgendaOfNoBookingsIsEmpty()
        {
            RespondWith(HttpStatusCode.OK, "{\"bookings\":[]}");

            Assert.That(Client.Appointments.Agenda(345, 7, DateTime.MinValue), Is.Empty);
        }

        [Test]
        public void AgendaSlotsReadsTheSlotsObject()
        {
            RespondWith(HttpStatusCode.OK, "{\"slots\":[" + CapacitySlot + "]}");

            Slot[] slots = Client.Appointments.AgendaSlots(904, 7, DateTime.MinValue);

            Assert.That(slots, Has.Length.EqualTo(1));
            Assert.That(slots[0].id, Is.EqualTo(149631371));
            Assert.That(slots[0].title, Is.EqualTo("Rem test"));
            Assert.That(slots[0].start, Is.EqualTo(new DateTime(2035, 1, 10, 14, 0, 0)));
        }

        [Test]
        public void AgendaSlotsAcceptsTheBookingsKey()
        {
            RespondWith(HttpStatusCode.OK, "{\"bookings\":[]}");

            Assert.That(Client.Appointments.AgendaSlots(904, 7, DateTime.MinValue), Is.Empty);
        }

        [Test]
        public void AvailableReadsTheSlotsObject()
        {
            RespondWith(HttpStatusCode.OK, "{\"slots\":[" + FreeSlot + "]}");

            Appointment[] free = Client.Appointments.Available(345, new DateTime(2026, 11, 2, 9, 0, 0));

            Assert.That(free, Has.Length.EqualTo(1));
            Assert.That(free[0].id, Is.EqualTo(611531309));
            Assert.That(free[0].start, Is.EqualTo(new DateTime(2026, 11, 2, 9, 0, 0)));
            Assert.That(free[0].finish, Is.EqualTo("2026-11-02T13:00"));
        }

        [Test]
        public void AvailableAcceptsTheBookingsKey()
        {
            RespondWith(HttpStatusCode.OK, "{\"bookings\":[" + Booking + "]}");

            Assert.That(Client.Appointments.Available(345, DateTime.Now).Select(a => a.id), Is.EqualTo(new[] { 101 }));
        }

        [Test]
        public void ListResponsesStillAcceptABareArray()
        {
            RespondWith(HttpStatusCode.OK, "[" + Booking + "]");

            Assert.That(Client.Appointments.Agenda(345, 7, DateTime.MinValue).Select(a => a.id),
                Is.EqualTo(new[] { 101 }));
            Assert.That(Client.Appointments.Available(345, DateTime.Now).Select(a => a.id), Is.EqualTo(new[] { 101 }));
        }

        [Test]
        public void ListResponsesWithoutAListAreEmpty()
        {
            RespondWith(HttpStatusCode.OK, "{}");

            Assert.That(Client.Appointments.Available(345, DateTime.Now), Is.Empty);
        }
    }
}
