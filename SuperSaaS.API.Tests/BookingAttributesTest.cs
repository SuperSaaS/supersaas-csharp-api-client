using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Tests
{
    /// <summary>Booking attributes sent on create and update, and read back from the API.</summary>
    [TestFixture]
    public class BookingAttributesTest : FakeHttpFixture
    {
        private const string CapacityBooking =
            "{\"id\":201,\"slot_id\":149631371,\"created_on\":\"2026-10-27T08:15:00Z\",\"user_id\":7," +
            "\"created_by\":\"administrator\",\"waitlisted\":false,\"price\":2500,\"deleted\":false," +
            "\"updated_on\":\"2026-10-27T08:20:03Z\",\"updated_by\":\"administrator\",\"status\":5," +
            "\"status_message\":\"Paid\",\"discount\":250,\"credit_consumed\":1000," +
            "\"full_name\":\"Tester Test\",\"email\":\"test@example.com\",\"description\":\"Front row\"," +
            "\"field_1_r\":\"Vegetarian\",\"super_field\":\"VIP\",\"quantity\":3,\"form_id\":88," +
            "\"price_cents\":2500,\"meeting_url\":\"https://meet.example/abc\"}";

        private const string ServiceBooking =
            "{\"id\":301,\"start\":\"2026-11-02T09:00\",\"finish\":\"2026-11-02T10:00\",\"service_id\":12," +
            "\"created_on\":\"2026-10-27T08:15:00Z\",\"user_id\":7,\"service_name\":\"Haircut\"," +
            "\"created_by\":\"administrator\",\"price\":null,\"deleted\":false,\"resources\":[611,612]}";

        private const string ResourceBooking =
            "{\"id\":401,\"start\":\"2026-11-02T09:00\",\"finish\":\"2026-11-02T10:00\",\"resource_id\":611," +
            "\"created_on\":\"2026-10-27T08:15:00Z\",\"user_id\":7,\"res_name\":\"Meeting Room 1\"," +
            "\"created_by\":\"administrator\",\"price\":null,\"deleted\":false,\"appointment_id\":301}";

        [Test]
        public void CreateSendsDescriptionQuantityAndFormId()
        {
            RespondWith(HttpStatusCode.Created, "", Host + "/api/bookings/201.json");

            Client.Appointments.Create(904, 7, Attributes());

            Assert.That(LastBody, Does.StartWith("{\"booking\":{"));
            AssertSendsBookingAttributes();
        }

        [Test]
        public void UpdateSendsDescriptionQuantityAndFormId()
        {
            Client.Appointments.Update(904, 201, Attributes());

            Assert.That(LastBody, Does.StartWith("{\"booking\":{"));
            AssertSendsBookingAttributes();
        }

        [Test]
        public void UnlistedAttributesAreStillSent()
        {
            Client.Appointments.Update(904, 201, new Dictionary<string, string> { { "custom_key", "kept" } });

            Assert.That(LastBody, Is.EqualTo("{\"booking\":{\"custom_key\":\"kept\"}}"));
        }

        [Test]
        public void ReadingACapacityBookingFillsTheBookingAttributes()
        {
            RespondWith(HttpStatusCode.OK, CapacityBooking);

            Appointment booking = Client.Appointments.Get(904, 201);

            Assert.Multiple(() =>
            {
                Assert.That(booking.id, Is.EqualTo(201));
                Assert.That(booking.slot_id, Is.EqualTo(149631371));
                Assert.That(booking.description, Is.EqualTo("Front row"));
                Assert.That(booking.quantity, Is.EqualTo(3));
                Assert.That(booking.form_id, Is.EqualTo(88));
                Assert.That(booking.price_cents, Is.EqualTo(2500));
                Assert.That(booking.status, Is.EqualTo("5"));
                Assert.That(booking.status_message, Is.EqualTo("Paid"));
                Assert.That(booking.discount, Is.EqualTo(250));
                Assert.That(booking.credit_consumed, Is.EqualTo(1000));
                Assert.That(booking.meeting_url, Is.EqualTo("https://meet.example/abc"));
                Assert.That(booking.field_1_r, Is.EqualTo("Vegetarian"));
                Assert.That(booking.super_field, Is.EqualTo("VIP"));
                Assert.That(booking.waitlisted, Is.False);
                Assert.That(booking.updated_by_name, Is.EqualTo("administrator"));
            });
        }

        [Test]
        public void ReadingAServiceBookingFillsServiceAndResources()
        {
            RespondWith(HttpStatusCode.OK, ServiceBooking);

            Appointment booking = Client.Appointments.Get(905, 301);

            Assert.Multiple(() =>
            {
                Assert.That(booking.service_id, Is.EqualTo(12));
                Assert.That(booking.service_name, Is.EqualTo("Haircut"));
                Assert.That(booking.resources, Is.EqualTo(new[] { 611, 612 }));
                Assert.That(booking.appointment_id, Is.Null);
            });
        }

        [Test]
        public void ReadingAResourceBookingLeavesUnsentAttributesNull()
        {
            RespondWith(HttpStatusCode.OK, ResourceBooking);

            Appointment booking = Client.Appointments.Get(345, 401);

            Assert.Multiple(() =>
            {
                Assert.That(booking.appointment_id, Is.EqualTo(301));
                Assert.That(booking.quantity, Is.Null);
                Assert.That(booking.form_id, Is.Null);
                Assert.That(booking.price_cents, Is.Null);
                Assert.That(booking.status_message, Is.Null);
                Assert.That(booking.resources, Is.Null);
            });
        }

        private void AssertSendsBookingAttributes()
        {
            Assert.That(LastBody, Does.Contain("\"description\":\"Front row\""));
            Assert.That(LastBody, Does.Contain("\"slot_id\":\"149631371\""));
            Assert.That(LastBody, Does.Contain("\"quantity\":\"3\""));
            Assert.That(LastBody, Does.Contain("\"form_id\":\"88\""));
        }

        private static Dictionary<string, string> Attributes()
        {
            return new Dictionary<string, string>
            {
                { "full_name", "Tester Test" },
                { "description", "Front row" },
                { "slot_id", "149631371" },
                { "quantity", "3" },
                { "form_id", "88" }
            };
        }
    }
}
