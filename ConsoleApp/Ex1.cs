using AzureStorageLib;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ConsoleApp
{
    public class Ex1
    {
        AzureTable _az = null;
        AdvWorks _avw = null;
        AdventureWorksContext _db;

        public Ex1(AzureTable az, AdvWorks avw, AdventureWorksContext db)
        {
            _az = az;
            _avw = avw;
            _db = db;
        }

        public void Test2()
        {
            var tables = _az.ListTables();
            foreach (var t in tables)
                Console.WriteLine(t.Name);
        }

        public void Test3()
        {
            _az.GetCreateTable("Customers");
            //az.GetCreateTable("Orders");
            //az.GetCreateTable("SalesOrders");

            var tables = _az.ListTables();
            foreach (var t in tables)
                Console.WriteLine(t.Name);

        }

        public void Test4()
        {
            var customers = _avw.GetCustomersFromDB();
            _avw.InsertMergeCustomer(customers);
        }

        public void Test5()
        {
            var customers = _avw.GetAllCustomers();
            Console.WriteLine($"Customers count: {customers.Count()}");
        }

        public void Test6()
        {
            var customers = _avw.GetCustomers();
            Console.WriteLine($"Customers count: {customers.Count()}");
        }

        public void Test7()
        {
            var customers = _db.Customer.ToList();
            Console.WriteLine($"Customers from DB count: {customers.Count()}");
        }
    }
}
