using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLib
{
    public class AzureTable
    {
        public AzureTable() : this(null) { }

        public AzureTable(string connectionString)
        {
            this.ConnectionString = connectionString;

            bool success = false;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                StorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
                success = true;
            } else
            {
                success = CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount account);
                StorageAccount = account;
            }
            if (success)
            {
                TableClient = StorageAccount.CreateCloudTableClient();
                IsConnected = true;
            }
        }

        public string ConnectionString { get; private set; }
        public bool IsConnected { get; private set; }
        public CloudStorageAccount StorageAccount { get; private set; }
        public CloudTableClient TableClient { get; private set; }
        public async Task<IEnumerable<CloudTable>> ListTablesAsync()
        {
            var tables = new List<CloudTable>();
            if (IsConnected)
            {
                TableContinuationToken tct = null;
                do
                {
                    var resp = await TableClient.ListTablesSegmentedAsync(tct);
                    tct = resp.ContinuationToken;
                    tables.AddRange(resp.Results);
                } while (tct != null);
            }
            return tables;
        }
        public IEnumerable<CloudTable> ListTables()
        {
            return Task.Run(async () => await ListTablesAsync()).Result;
        }
        public async Task<CloudTable> GetCreateTableAsync(string tableName)
        {
            var table = TableClient.GetTableReference(tableName);
            if (await table.ExistsAsync())
                return table;
            else if (await table.CreateIfNotExistsAsync())
                return table;
            else return null;
        }
        public CloudTable GetCreateTable(string tableName)
        {
            return Task.Run(async () => await GetCreateTableAsync(tableName)).Result;
        }

        public async Task AddEntityAsync(IEnumerable<TableEntity> entities, string tableName)
        {
            var table = await this.GetCreateTableAsync(tableName);
            var op = new TableBatchOperation();

            int i = 0;
            foreach (var e in entities) {
                if (++i % 100 == 0) {
                    await table.ExecuteBatchAsync(op);
                    op.Clear();
                }
                op.InsertOrMerge(e);
            }
            if (op.Count > 0)
                await table.ExecuteBatchAsync(op);
        }

        public void AddEntity(IEnumerable<TableEntity> entities, string tableName)
        {
            Task.Run(async () => await AddEntityAsync(entities, tableName)).Wait();
        }
    }
}
