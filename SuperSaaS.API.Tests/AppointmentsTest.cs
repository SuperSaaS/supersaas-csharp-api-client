using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture]
    public class AppointmentsTest : TestModeFixture
    {
        [Test]
        public void GetTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Get(123, 456));
        }

        [Test]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.List(123, true, DateTime.Now, 10));
        }

        [Test]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Create(123, 789, AppointmentArgs()));
        }

        [Test]
        public void UpdateTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Update(123, 456, AppointmentArgs()));
        }

        [Test]
        public void DeleteTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Delete(123, 456));
        }

        [Test]
        public void AgendaTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Agenda(123, 456, DateTime.Now));
        }

        [Test]
        public void AgendaSlotsTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.AgendaSlots(123, 456, DateTime.Now));
        }

        [Test]
        public void AvailableTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Available(123, DateTime.Now));
            Assert.DoesNotThrow(() => Client.Appointments.Available(123, DateTime.Now, 10, "Test Resource", true, 15));
        }

        [Test]
        public void ChangesTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.Changes(123, DateTime.Now));
        }

        [Test]
        public void ChangesSlotsTest()
        {
            Assert.DoesNotThrow(() => Client.Appointments.ChangesSlots(123, DateTime.Now));
        }

        private static Dictionary<string, string> AppointmentArgs()
        {
            return new Dictionary<string, string>
            {
                { "description", "Testing" },
                { "name", "Test" },
                { "email", "test@example.com" },
                { "full_name", "Tester Test" },
                { "address", "123 St, City" },
                { "mobile", "555-5555" },
                { "phone", "555-5555" },
                { "country", "FR" },
                { "field_1", "f 1" },
                { "field_2", "f 2" },
                { "field_1_r", "f 1 r" },
                { "field_2_r", "f 2 r" },
                { "super_field", "sf" }
            };
        }
    }
}
