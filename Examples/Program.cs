using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SuperSaaS.API;
using SuperSaaS.API.Models;

namespace Examples
{
    /// <summary>
    /// Demonstrates common SuperSaaS API requests.
    ///
    /// Configure it with environment variables:
    ///   SSS_API_ACCOUNT_NAME, SSS_API_KEY    account name and API key (required)
    ///   SSS_API_HOST                         server, defaults to https://www.supersaas.com
    ///   SSS_API_USER_ID                      user for the booking examples
    ///   SSS_API_CAPACITY_SCHEDULE_ID, SSS_API_CAPACITY_SLOT_ID
    ///   SSS_API_RESOURCE_SCHEDULE_ID, SSS_API_SERVICE_SCHEDULE_ID
    ///
    /// IMPORTANT: some examples are destructive or may incur a cost, such as deleting users or adding
    /// appointments.
    /// </summary>
    internal static class Program
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        private static readonly Random random = new Random();

        private static readonly int userId = IdFromEnvironment("SSS_API_USER_ID");
        private static readonly int capacityScheduleId = IdFromEnvironment("SSS_API_CAPACITY_SCHEDULE_ID");
        private static readonly int capacitySlotId = IdFromEnvironment("SSS_API_CAPACITY_SLOT_ID");
        private static readonly int resourceScheduleId = IdFromEnvironment("SSS_API_RESOURCE_SCHEDULE_ID");
        private static readonly int serviceScheduleId = IdFromEnvironment("SSS_API_SERVICE_SCHEDULE_ID");

        private static Client client = null!;

        public static int Main()
        {
            Console.WriteLine("SuperSaaS Schedules Example");

            var configuration = new Configuration();
            string? host = Environment.GetEnvironmentVariable("SSS_API_HOST");
            if (!string.IsNullOrEmpty(host))
            {
                configuration.Host = host;
            }
            if (string.IsNullOrEmpty(configuration.AccountName) || string.IsNullOrEmpty(configuration.ApiKey))
            {
                Console.Error.WriteLine("Set SSS_API_ACCOUNT_NAME and SSS_API_KEY first.");
                return 1;
            }
            client = new Client(configuration);

            try
            {
                ListSchedulesAndResources();
                ListUsers();
                CreateUser();

                if (userId > 0)
                {
                    CreateUpdateDeleteBooking(userId);
                    CreateReservation(userId);
                    CreateAppointment(userId);
                    UpdateDeleteUser(userId);
                }

                ListChanges();
            }
            catch (SSSException e)
            {
                Console.WriteLine("Error!");
                Console.WriteLine(e.Message);
                return 1;
            }
            return 0;
        }

        private static void ListSchedulesAndResources()
        {
            Console.WriteLine("listing schedules...");
            Schedule[] schedules = client.Schedules.List();
            for (int i = 0; i < schedules.Length; i++)
            {
                Console.WriteLine(i + " " + schedules[i].name);
                try
                {
                    foreach (Resource resource in client.Schedules.Resources(schedules[i].id))
                    {
                        Console.WriteLine("     " + resource.name);
                    }
                }
                catch (SSSException e) when (e.Message.Contains("not available for Capacity type"))
                {
                    // Capacity schedules have no resources.
                }
            }
        }

        private static void CreateUpdateDeleteBooking(int forUserId)
        {
            if (capacityScheduleId == 0 || capacitySlotId == 0)
            {
                return;
            }

            var data = new Dictionary<string, string>
            {
                { "slot_id", capacitySlotId.ToString() },
                { "name", "example" }
            };
            Console.WriteLine("creating new booking...");
            Appointment model = client.Appointments.Create(capacityScheduleId, forUserId, data);

            Console.WriteLine("updating booking " + model.id + "...");
            client.Appointments.Update(capacityScheduleId, model.id, new Dictionary<string, string>
            {
                { "name", "updated" }
            });

            Console.WriteLine("deleting booking...");
            client.Appointments.Delete(capacityScheduleId, model.id);
        }

        private static void CreateReservation(int forUserId)
        {
            if (resourceScheduleId == 0)
            {
                return;
            }
            Console.WriteLine("creating new reservation...");
            Appointment appointment = client.Appointments.Create(resourceScheduleId, forUserId, AppointmentData());
            Console.WriteLine("     id: " + appointment.id);
        }

        private static void CreateAppointment(int forUserId)
        {
            if (serviceScheduleId == 0)
            {
                return;
            }
            Console.WriteLine("creating new appointment...");
            Appointment appointment = client.Appointments.Create(serviceScheduleId, forUserId, AppointmentData());
            Console.WriteLine("     id: " + appointment.id);
        }

        private static Dictionary<string, string> AppointmentData()
        {
            DateTime start = DateTime.Now.Date.AddDays(random.Next(1, 90)).AddHours(9);
            return new Dictionary<string, string>
            {
                { "full_name", "example" },
                { "description", "desc" },
                { "address", "addr" },
                { "phone", "555-5556" },
                { "mobile", "555-5555" },
                { "email", "example@example.com" },
                { "name", "example@example.com" },
                { "start", start.ToString(DateTimeFormat, CultureInfo.InvariantCulture) },
                { "finish", start.AddHours(1).ToString(DateTimeFormat, CultureInfo.InvariantCulture) }
            };
        }

        private static void ListChanges()
        {
            int id = resourceScheduleId > 0 ? resourceScheduleId : serviceScheduleId;
            Console.WriteLine("listing bookings changes...");
            if (id > 0)
            {
                Changes changes = client.Appointments.Changes(id, DateTime.Now.AddDays(-30));
                foreach (Appointment booking in changes.bookings ?? Array.Empty<Appointment>())
                {
                    Console.WriteLine(" " + booking.id);
                }
            }

            Console.WriteLine("listing slots changes...");
            if (capacityScheduleId > 0)
            {
                Changes changes = client.Appointments.ChangesSlots(capacityScheduleId, DateTime.Now.AddDays(-30));
                foreach (Slot slot in changes.slots ?? Array.Empty<Slot>())
                {
                    Console.WriteLine(" " + slot.id);
                }
            }
        }

        private static void ListUsers()
        {
            Console.WriteLine("listing users...");
            User[] users = client.Users.List(true, 25);
            for (int i = 0; i < users.Length; i++)
            {
                Console.WriteLine(i + " " + users[i].name + " (" + users[i].id + ")");
            }
        }

        private static void CreateUser()
        {
            var data = new Dictionary<string, string>
            {
                { "full_name", "Example" },
                { "name", RandomString(12) + "@example.com" },
                { "email", RandomString(12) + "@example.com" },
                { "password", RandomString(16) }
            };
            Console.WriteLine("creating new user...");
            client.Users.Create(data);
        }

        private static void UpdateDeleteUser(int id)
        {
            Console.WriteLine("Getting user " + id + "...");
            User user = client.Users.Get(id);
            Console.WriteLine(" " + user.name);

            var data = new Dictionary<string, string>
            {
                { "country", "FR" },
                { "address", "Rue 1" }
            };
            Console.WriteLine("Updating user " + id + "...");
            client.Users.Update(id, data);

            Console.WriteLine("Deleting user " + id + "...");
            client.Users.Delete(user.id);
        }

        private static int IdFromEnvironment(string name)
        {
            return int.TryParse(Environment.GetEnvironmentVariable(name), out int id) ? id : 0;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
