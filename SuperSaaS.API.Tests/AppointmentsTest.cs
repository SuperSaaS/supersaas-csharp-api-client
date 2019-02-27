using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SuperSaaS.API.Tests
{
    [TestFixture()]
    public class AppointmentsTest
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
        public void GetTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Get(123, 456); });
        }

        [Test()]
        public void ListTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.List(123, true, DateTime.Now, 10); });
        }

        [Test()]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Create(123, 789, this.AppointmentArgs()); });
        }

        [Test()]
        public void UpdateTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Update(123, 456, this.AppointmentArgs()); });
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Delete(123, 456); });
        }

        [Test()]
        public void AgendaTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Agenda(123, 456, DateTime.Now); });
        }

        [Test()]
        public void AgendaSlotsTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.AgendaSlots(123, 456, DateTime.Now); });
        }

        [Test()]
        public void AvailableTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Available(123, DateTime.Now); });
            Assert.DoesNotThrow(() => { this.Client.Appointments.Available(123, DateTime.Now, 10, "Test Resource", true, 15); });
        }

        [Test()]
        public void ChangesTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.Changes(123, DateTime.Now); });
        }

        [Test()]
        public void ChangesSlotsTest()
        {
            Assert.DoesNotThrow(() => { this.Client.Appointments.ChangesSlots(123, DateTime.Now); });
        }

        private Dictionary<string, string> AppointmentArgs()
        {
            return new Dictionary<string, string> {
                {"description", "Testing"},
                {"name", "Test"},
                {"email", "test@example.com"},
                {"full_name", "Tester Test"},
                {"address", "123 St, City"},
                {"mobile", "555-5555"},
                {"phone", "555-5555"},
                {"country", "FR"},
                {"field_1", "f 1"},
                {"field_2", "f 2"},
                {"field_1_r", "f 1 r"},
                {"field_2_r", "f 2 r"},
                {"super_field", "sf"}
            };
        }
    }
}
