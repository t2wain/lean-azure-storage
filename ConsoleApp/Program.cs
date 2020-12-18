using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AzTest();
            //ConfigTest();
        }

        static void ConfigTest()
        {
            StartupConfig.LoadAppSettings();
        }

        static void AzTest()
        {
            IServiceProvider services = StartupConfig.ConfigureServices();
            var ex1 = services.GetService<Ex1>();
            //ex1.Test2();
            //ex1.Test3();
            //ex1.Test4();
            ex1.Test5();
            ex1.Test6();
            ex1.Test7();
        }
    }
}
