using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class StartupConfig
    {
        public static IConfigurationRoot LoadAppSettings()
        {
            var configRoot = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json");

            var env = Environment.GetEnvironmentVariable("RUNTIME_ENVIRONMENT");
            if (env == "Development")
                configRoot.AddJsonFile("appSettings.Development.json");

            return configRoot.Build();
        }
    }
}
