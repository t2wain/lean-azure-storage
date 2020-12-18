using AzureStorageLib;
using System;
using System.Linq;

namespace ConsoleApp
{
    public class Ex1
    {
        string connStr = "";

        public Ex1()
        {
            var root = StartupConfig.LoadAppSettings();
            connStr = root["AzStorage:ConnectionString"];
        }

        public void Test2()
        {
            var az = GetAz();
            var tables = az.ListTables();
            foreach (var t in tables)
                Console.WriteLine(t.Name);
        }

        public void Test3()
        {
            var az = GetAz();
            az.GetCreateTable("Customers");
            //az.GetCreateTable("Orders");
            //az.GetCreateTable("SalesOrders");

            var tables = az.ListTables();
            foreach (var t in tables)
                Console.WriteLine(t.Name);

        }

        public void Test4()
        {
            var avw = new AdvWorks(GetAz());
            var customers = avw.GetCustomersFromDB();
            avw.InsertMergeCustomer(customers);
        }

        public void Test5()
        {
            var avw = new AdvWorks(GetAz());
            var customers = avw.GetAllCustomers();
            Console.WriteLine($"Customers count: {customers.Count()}");
        }

        public void Test6()
        {
            var avw = new AdvWorks(GetAz());
            var customers = avw.GetCustomers();
            Console.WriteLine($"Customers count: {customers.Count()}");
        }

        AzureTable GetAz()
        {
            return new AzureTable(connStr);
        }
    }
}
