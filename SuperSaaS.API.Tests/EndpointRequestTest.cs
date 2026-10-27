using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    /// <summary>
    /// Checks the requests the endpoint groups build, through a fake message Handler.
    /// </summary>
    [TestFixture]
    public class EndpointRequestTest : FakeHttpFixture
    {
        [Test]
        public void AvailableRequestsFreeTimesOfTheSchedule()
        {
            Client.Appointments.Available(123, new DateTime(2026, 5, 4, 13, 30, 0));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/free/123.json?from=2026-05-04%2013%3A30%3A00"));
        }

        [Test]
        public void AvailablePassesAllOptions()
        {
            Client.Appointments.Available(123, new DateTime(2026, 5, 4, 13, 30, 0), 45, "Room 1", true, 5);
            Assert.That(LastUrl, Is.EqualTo(Host +
                "/api/free/123.json?from=2026-05-04%2013%3A30%3A00&length=45&resource=Room%201&full=true&maxresults=5"));
        }

        [Test]
        public void FormatsDatesIndependentOfTheCurrentCulture()
        {
            CultureInfo original = CultureInfo.CurrentCulture;
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.DateTimeFormat.TimeSeparator = ".";
            culture.DateTimeFormat.DateSeparator = "/";
            try
            {
                CultureInfo.CurrentCulture = culture;
                Client.Appointments.Changes(7, new DateTime(2026, 1, 2, 3, 4, 5));
            }
            finally
            {
                CultureInfo.CurrentCulture = original;
            }
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/changes/7.json?from=2026-01-02%2003%3A04%3A05"));
        }

        [Test]
        public void CreateAppointmentReturnsIdFromLocationHeader()
        {
            RespondWith(HttpStatusCode.Created, "", Host + "/api/bookings/98765.json");

            Appointment appointment = Client.Appointments.Create(12, 34,
                new Dictionary<string, string> { { "full_name", "Zoë" } }, true, true);

            Assert.That(appointment.id, Is.EqualTo(98765));
            Assert.That(Client.GetResourceIdFromHeader(), Is.EqualTo(98765));
            Assert.That(Handler.LastRequest.Message.Method, Is.EqualTo(System.Net.Http.HttpMethod.Post));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/bookings.json?schedule_id=12&user_id=34&webhook=true&form=true"));
        }

        [Test]
        public void CreateAppointmentWithoutLocationHeaderReturnsIdZero()
        {
            Appointment appointment = Client.Appointments.Create(12, 34, new Dictionary<string, string>());
            Assert.That(appointment.id, Is.Zero);
        }

        [Test]
        public void ListAppointmentsBuildsQuery()
        {
            Client.Appointments.List(5, true, new DateTime(2026, 2, 3, 4, 5, 6), 10);
            Assert.That(LastUrl, Is.EqualTo(Host +
                "/api/bookings.json?schedule_id=5&from=2026-02-03%2004%3A05%3A06&form=true&limit=10"));
        }

        [Test]
        public void AgendaSlotsBuildsQuery()
        {
            Client.Appointments.AgendaSlots(5, 6, DateTime.MinValue);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/agenda/5.json?user=6&slot=true"));
        }

        [Test]
        public void UsersRequestsUseExpectedPaths()
        {
            Client.Users.Get("123fk");
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/users/123fk.json"));

            Client.Users.List(true, 10, 20);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/users.json?form=true&limit=10&offset=20"));

            Client.Users.Create(new Dictionary<string, string> { { "name", "a@b.c" } }, "9fk", true);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/users/9fk.json?webhook=true"));

            Client.Users.Delete(9);
            Assert.That(Handler.LastRequest.Message.Method, Is.EqualTo(System.Net.Http.HttpMethod.Delete));
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/users/9.json"));
        }

        [Test]
        public void SchedulesAndFormsRequestsUseExpectedPaths()
        {
            Client.Schedules.Resources(42);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/resources.json?schedule_id=42"));

            Client.Forms.List(8, DateTime.MinValue);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/forms.json?form_id=8"));

            Client.Forms.Get(9);
            Assert.That(LastUrl, Is.EqualTo(Host + "/api/forms.json?id=9"));
        }
    }
}
