using System;
using System.Linq;
using System.Collections.Generic;
using SuperSaaS.API;
using SuperSaaS.API.Models;

namespace Examples
{
    class MainClass
    {
        private const string HOST = "";
        private const string ACCOUNT = "";
        private const string API_KEY = "";
        private const int CAPACITY_SCHEDULE_ID = 0;
        private const int CAPACITY_SLOT_ID = 0;
        private const int RESOURCE_SCHEDULE_ID = 0;
        private const int SERVICE_SCHEDULE_ID = 0;
        private const int USER_ID = 0;

        private static Client client;
        private static Random random = new Random();

        public static void Main(string[] args)
        {
            Console.WriteLine("SuperSaaS Schedules Example");

            client = new Client(ACCOUNT, API_KEY, HOST);

            try
            {
                listSchedulesAndResources();
                listUsers();
                createUser();

                if (USER_ID > 0)
                {
                    createUpdateDeleteBooking(USER_ID);
                    createUpdateDeleteReservation(USER_ID);
                    createUpdateDeleteAppointment(USER_ID);
                    updateDeleteUser(USER_ID);
                }

                listChanges();
            } catch (SSSException e)
            {
                Console.WriteLine("Error!");
                Console.WriteLine(e.Message);
            }

        }

        private static void listSchedulesAndResources()
        {
            Console.WriteLine("listing schedules...");
            Schedule[] schedules = client.Schedules.List();
            for (int i = 0; i < schedules.Length; i++)
            {
                Console.WriteLine(i.ToString() + " " + schedules[i].name);
                try
                {
                    Resource[] resources = client.Schedules.Resources(schedules[i].id);
                    for (int j = 0; j < resources.Length; j++)
                    {
                        Console.WriteLine("     " + resources[j].name);
                    }
                }
                catch (SSSException e) { 
                    if (e.Message.IndexOf("not available for Capacity type") < 0)
                    {
                        throw;
                    }
                }
            }
        }

        private static void createUpdateDeleteBooking(int userId)
        {
            if (CAPACITY_SCHEDULE_ID == 0 || CAPACITY_SLOT_ID == 0) return;

            var data = new Dictionary<string, string>
            {
                { "slot_id", CAPACITY_SLOT_ID.ToString() },
                { "name", "example" }
            };
            Console.WriteLine("creating new booking...");
            Appointment model = client.Appointments.Create(CAPACITY_SCHEDULE_ID, userId, data);

            data = new Dictionary<string, string>
            {
                { "name", "updated" }
            };
            Console.WriteLine("updating booking " + model.id + " for slot " + model.slot_id + "...");
            client.Appointments.Update(CAPACITY_SCHEDULE_ID, model.id, data);
            Console.WriteLine("     name: " + model.name);

            Console.WriteLine("deleting booking...");
            client.Appointments.Delete(CAPACITY_SCHEDULE_ID, model.id);
        }

        private static void createUpdateDeleteReservation(int userId)
        {
            if (RESOURCE_SCHEDULE_ID == 0) return;

            int days = random.Next(1, 90);
            var data = new Dictionary<string, string>
            {
                { "full_name", "example" },
                { "description", "desc" },
                { "address", "addr" },
                { "phone", "555-5556" },
                { "mobile", "555-5555" },
                { "email", "example@example.com" },
                { "name", "example@example.com" },
                { "start",  DateTime.Now.AddDays(days).ToString("YYYY-MM-DD HH:MM:SS") },
                { "finish",  DateTime.Now.AddDays(days).AddHours(1).ToString("YYYY-MM-DD HH:MM:SS") }
            };
            Console.WriteLine("creating new reservation...");
            Appointment appointment = client.Appointments.Create(RESOURCE_SCHEDULE_ID, userId, data);
        }

        private static void createUpdateDeleteAppointment(int userId)
        {
            if (SERVICE_SCHEDULE_ID == 0) return;

            int days = random.Next(1, 90);
            var data = new Dictionary<string, string>
            {
                { "full_name", "example" },
                { "description", "desc" },
                { "address", "addr" },
                { "phone", "555-5556" },
                { "mobile", "555-5555" },
                { "email", "example@example.com" },
                { "name", "example@example.com" },
                { "start",  DateTime.Now.AddDays(days).ToString("YYYY-MM-DD HH:MM:SS") },
                { "finish",  DateTime.Now.AddDays(days).AddHours(1).ToString("YYYY-MM-DD HH:MM:SS") }
            };
            Console.WriteLine("creating new appointment...");
            Appointment appointment = client.Appointments.Create(SERVICE_SCHEDULE_ID, userId, data);
        }

        private static void listChanges()
        {
            int id = RESOURCE_SCHEDULE_ID > 0 ? RESOURCE_SCHEDULE_ID : SERVICE_SCHEDULE_ID;
            Console.WriteLine("listing bookings changes...");
            if (id > 0)
            {
                Changes changes = client.Appointments.Changes(id, DateTime.Now.AddDays(-30));
                for (int i = 0; i < changes.bookings.Length; i++)
                {
                    Console.WriteLine(" " + changes.bookings[i].id);
                }
            }

            Console.WriteLine("listing slots changes...");
            if (CAPACITY_SCHEDULE_ID > 0)
            {
                Changes changes = client.Appointments.ChangesSlots(CAPACITY_SCHEDULE_ID, DateTime.Now.AddDays(-30));
                for (int i = 0; i < changes.slots.Length; i++)
                {
                    Console.WriteLine(" " + changes.slots[i].id);
                }
            }
        }

        private static void listUsers()
        {
            Console.WriteLine("listing users...");
            User[] users = client.Users.List(true, 25);
            for (int i = 0; i < users.Length; i++)
            {
                Console.WriteLine(i.ToString() + " " + users[i].name + " (" + users[i].id + ")");
            }
        }

        private static void createUser()
        {
            var data = new Dictionary<string, string>
            {
                { "full_name", "Example" },
                { "name", RandomString(12) + "@example.com" },
                { "email", RandomString(12) + "@example.com" },
                { "password", "example" }
            };
            Console.WriteLine("creating new user...");
            client.Users.Create(data);

        }

        private static void updateDeleteUser(int userId)
        {
            Console.WriteLine("Getting user " + userId + "...");
            User user = client.Users.Get(userId);
            Console.WriteLine(" " + user.name);

            var data = new Dictionary<string, string>
            {
                { "country", "FR" },
                { "address", "Rue 1" }
            };
            Console.WriteLine("Updating user " + userId + "...");
            client.Users.Update(userId, data);

            Console.WriteLine("Deleting user " + userId + "...");
            client.Users.Delete(user.id);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
