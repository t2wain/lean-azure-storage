using AzureStorageLib;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class StartupConfig
    {
        static IConfigurationRoot _root = null;
        static IServiceProvider _services = null;

        public static IConfigurationRoot LoadAppSettings()
        {
            if (_root == null)
            {

                var configRoot = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json");

                var env = Environment.GetEnvironmentVariable("RUNTIME_ENVIRONMENT");
                if (!String.IsNullOrWhiteSpace(env))
                    configRoot.AddJsonFile($"appsettings.{env}.json");

                _root = configRoot.Build();
            }
            return _root;
        }

        public static IServiceProvider ConfigureServices()
        {
            if (_services == null)
            {
                var cfg = LoadAppSettings();
                _services = new ServiceCollection()
                    .AddLogging()
                    .AddDbContext<AdventureWorksContext>(option =>
                    {
                        option.UseSqlServer(cfg.GetConnectionString("DefaultConnection"));
                    })
                    .AddSingleton(new AzureTable(cfg["AzStorage:ConnectionString"]))
                    .AddSingleton<AdvWorks>()
                    .AddSingleton<Ex1>()
                    .BuildServiceProvider();
            }
            return _services;
        }
    }
}
