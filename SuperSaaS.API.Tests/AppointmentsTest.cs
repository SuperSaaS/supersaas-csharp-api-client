using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SuperSaaS.API;
using SuperSaaS.API.Models;

#nullable enable

namespace SuperSaaS.API.Tests
{
    [TestClass]
    public class AppointmentsTest
    {
        private Client? Client { get; set; }
        private readonly int scheduleId = 123;
        private readonly int appointmentId = 456;
        private readonly string userId = "789";
        private readonly DateTime testTime = new DateTime(2025, 1, 1, 12, 0, 0);

        [TestInitialize]
        public void Init()
        {
            Configuration config = new Configuration
            {
                AccountName = "accnt",
                ApiKey = "xxxxxxxxxxxxxxxxxxxx",
                Test = true
            };
            Client = new Client(config);
        }

        [TestMethod]
        public void GetTest()
        {
            var appointment = Client!.Appointments.Get(scheduleId, appointmentId);
            Assert.IsNotNull(appointment);
            Assert.IsInstanceOfType(appointment, typeof(Appointment));
        }

        [TestMethod]
        public void ListTest()
        {
            var appointments = Client!.Appointments.List(
                scheduleId: scheduleId,
                form: true,
                startTime: testTime,
                finish: testTime.AddHours(1),
                limit: 10
            );

            Assert.IsNotNull(appointments);
            Assert.IsInstanceOfType(appointments, typeof(Appointment[]));
        }

        [TestMethod]
        public void CreateTest()
        {
            var attributes = AppointmentArgs();
            var appointment = Client!.Appointments.Create(
                scheduleId: scheduleId, 
                userId: userId, 
                attributes: attributes,
                form: true,
                webhook: true
            );
            Assert.IsNotNull(appointment);
            Assert.IsInstanceOfType(appointment, typeof(Appointment));
        }

        [TestMethod]
        public void UpdateTest()
        {
            var attributes = AppointmentArgs();
            var appointment = Client!.Appointments.Update(
                scheduleId: scheduleId, 
                appointmentId: appointmentId, 
                attributes: attributes,
                form: true,
                webhook: true
            );
            Assert.IsNotNull(appointment);
            Assert.IsInstanceOfType(appointment, typeof(Appointment));
        }

        [TestMethod]
        public void DeleteTest()
        {
            Client!.Appointments.Delete(
                scheduleId: scheduleId, 
                appointmentId: appointmentId,
                webhook: true
            );
        }

        [TestMethod]
        public void AgendaTest()
        {
            var appointments = Client!.Appointments.Agenda(
                scheduleId: scheduleId, 
                user: userId, 
                fromTime: testTime, 
                slot: true
            );
            Assert.IsNotNull(appointments);
            Assert.IsInstanceOfType(appointments, typeof(Appointment[]));
        }

        [TestMethod]
        public void AvailableTest()
        {

            var appointments1 = Client!.Appointments.Available(
                scheduleId: scheduleId, 
                fromTime: testTime
            );
            Assert.IsNotNull(appointments1);
            Assert.IsInstanceOfType(appointments1, typeof(Appointment[]));

            var appointments2 = Client.Appointments.Available(
                scheduleId: scheduleId, 
                fromTime: testTime, 
                lengthMinutes: 10, 
                resource: "Test Resource", 
                full: true, 
                limit: 15
            );
            Assert.IsNotNull(appointments2);
            Assert.IsInstanceOfType(appointments2, typeof(Appointment[]));
        }

        [TestMethod]
        public void ChangesTest()
        {
            var changes = Client!.Appointments.Changes(
                scheduleId: scheduleId,
                fromTime: testTime,
                to: testTime.AddDays(1),
                slot: true,
                user: userId,
                limit: 10,
                offset: 0
            );
            Assert.IsNotNull(changes);
            Assert.IsInstanceOfType(changes, typeof(Changes));
        }

        [TestMethod]
        public void RangeTest()
        {
            var appointments = Client!.Appointments.Range(
                scheduleId: scheduleId,
                today: true,
                fromTime: testTime,
                to: testTime.AddDays(1),
                slot: true,
                user: userId,
                resourceId: 1,
                serviceId: 1,
                limit: 10,
                offset: 0
            );

            Assert.IsNotNull(appointments);
            Assert.IsInstanceOfType(appointments, typeof(Appointment[]));
        }
        private static Dictionary<string, string> AppointmentArgs()
        {
            var testTime = new DateTime(2025, 1, 1, 12, 0, 0);

            return new Dictionary<string, string>
            {
                {"address", "123 St, City"},
                {"country", "FR"},
                {"created_by", "test_user"},
                {"description", "Testing"},
                {"email", "test@example.com"},
                {"field_1", "f 1"},
                {"field_2", "f 2"},
                {"field_1_r", "f 1 r"},
                {"field_2_r", "f 2 r"},
                {"finish", testTime.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss")},
                {"form_id", "1"},
                {"full_name", "Tester Test"},
                {"mobile", "555-5555"},
                {"phone", "555-5555"},
                {"price", "100.00"},
                {"price_cents", "10000"},
                {"quantity", "1"},
                {"res_name", "Test Resource"},
                {"resource_id", "1"},
                {"schedule_id", "123"},
                {"schedule_name", "Test Schedule"},
                {"service_id", "1"},
                {"service_name", "Test Service"},
                {"slot_id", "1"},
                {"start", testTime.ToString("yyyy-MM-dd HH:mm:ss")},
                {"status", "confirmed"},
                {"status_message", "Booking confirmed"},
                {"super_field", "sf"},
                {"updated_by", "test_user"},
                {"user_id", "789"},
                {"waitlisted", "false"},
                {"form", "true"},
                {"length", "10"},
                {"limit", "10"},
                {"maxresults", "15"},
                {"offset", "0"},
                {"slot", "true"},
                {"today", "true"},
                {"webhook", "true"}
            };
        }
    }
}
