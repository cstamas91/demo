using System;
using System.Configuration;

namespace Config.Client.Framework
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string dbName = ConfigurationManager.AppSettings["DbName"];
            Console.WriteLine(dbName);

            Console.ReadKey();
        }
    }
}
