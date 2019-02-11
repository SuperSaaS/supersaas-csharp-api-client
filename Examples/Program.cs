using System;
using SuperSaaS.API;
using SuperSaaS.API.Models;

namespace Examples
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("SuperSaaS Schedules Example");

            Client client = new Client();
            if (String.IsNullOrEmpty(client.AccountName))
            {
                Console.WriteLine("ERROR! Missing account credentials. Rerun the example app with your credentials in the ENV.");
            }

            Console.WriteLine("listing schedules...");
            Schedule[] schedules = client.Schedules.List();
            for (int i = 0; i < schedules.Length; i++) {
                Console.WriteLine(i.ToString() + " " + schedules[i].name);
            }
        }
    }
}
