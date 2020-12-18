using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;
using Core.Entities;
using AzureStorageLib.Entity;
using Microsoft.Azure.Cosmos.Table;

namespace AzureStorageLib
{
    public class AdvWorks
    {
        public const string TBL_CUSTOMERS = "Customers";

        AzureTable _az = null;
        AdventureWorksContext _db = null;

        public AdvWorks(AzureTable az, AdventureWorksContext db)
        {
            this._az = az;
            this._db = db;
        }

        public IEnumerable<CustomerEntity> GetCustomersFromDB()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Customer, CustomerEntity>());
            var mapper = new Mapper(config);

            var customers = this._db.Customer.ToList();
            var lst = new List<CustomerEntity>();
            foreach (var c in customers)
            {
                var e = mapper.Map<CustomerEntity>(c);
                e.PartitionKey = "Customer";
                e.RowKey = c.CustomerId.ToString();
                lst.Add(e);
            }
            return lst;
        }

        public void InsertMergeCustomer(IEnumerable<CustomerEntity> customers)
        {
            var az = this._az;
            az.AddEntity(customers, TBL_CUSTOMERS);
        }

        public IEnumerable<CustomerEntity> GetAllCustomers()
        {
            var az = this._az;
            var tbl = az.GetCreateTable(TBL_CUSTOMERS);
            var customers = tbl.ExecuteQuery(new TableQuery<CustomerEntity>());
            return customers.ToList();
        }

        public IEnumerable<CustomerEntity> GetCustomers()
        {
            var az = this._az;
            var tbl = az.GetCreateTable(TBL_CUSTOMERS);

            var q1 = tbl.CreateQuery<CustomerEntity>()
                .Where(x => x.LastName == "Li");
            var customers = q1.ToList();
            return customers;
        }
    }
}
