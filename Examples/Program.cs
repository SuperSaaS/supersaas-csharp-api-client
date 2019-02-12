using System;
using System.Collections.Generic;
using SuperSaaS.API;
using SuperSaaS.API.Models;

namespace Examples
{
    class MainClass
    {
        private const string HOST = "";
        private const string ACCOUNT = "";
        private const string PASSWORD = "";
        private const int CAPACITY_SCHEDULE_ID = 0;
        private const int CAPACITY_SLOT_ID = 0;
        private const int RESOURCE_SCHEDULE_ID = 0;
        private const int SERVICE_SCHEDULE_ID = 0;

        private static Client client;

        public static void Main(string[] args)
        {
            Console.WriteLine("SuperSaaS Schedules Example");

            client = new Client(ACCOUNT, PASSWORD, HOST);

            try
            {
                listSchedulesAndResources();
                listUsers();
                int userId = createUser();

                createUpdateDeleteBooking(userId);
                createUpdateDeleteReservation(userId);
                createUpdateDeleteAppointment(userId);

                updateDeleteUser(userId);
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

            var data = new Dictionary<string, string>
            {
                { "full_name", "example" },
                { "description", "desc" },
                { "address", "addr" },
                { "phone", "555-5556" },
                { "mobile", "555-5555" },
                { "email", "example@example.com" },
                { "name", "example@example.com" }
            };
            Console.WriteLine("creating new reservation...");
            Appointment appointment = client.Appointments.Create(RESOURCE_SCHEDULE_ID, userId, data);
        }

        private static void createUpdateDeleteAppointment(int userId)
        {
            if (SERVICE_SCHEDULE_ID == 0) return;
        }

        private static void listUsers()
        {
            Console.WriteLine("listing users...");
            User[] users = client.Users.List(true, 25);
            for (int i = 0; i < users.Length; i++)
            {
                Console.WriteLine(i.ToString() + " " + users[i].name);
            }
        }

        private static int createUser()
        {
            var data = new Dictionary<string, string>
            {
                { "full_name", "Example" },
                { "name", "example@example.com" },
                { "email", "example@example.com" },
                { "password", "example" }
            };
            Console.WriteLine("creating new user...");
            User user = client.Users.Create(data);
            return user.id;

        }

        private static void updateDeleteUser(int userId)
        {
            var data = new Dictionary<string, string>
            {
                { "country", "FR" },
                { "address", "Rue 1" }
            };
            Console.WriteLine("Updating user " + userId + "...");
            User user = client.Users.Update(userId, data);
            Console.WriteLine("     country: " + user.country);

            Console.WriteLine("Deleting user " + userId + "...");
            client.Users.Delete(user.id);
        }
    }
}
