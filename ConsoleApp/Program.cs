using AzureStorageLib;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var ex1 = new Ex1();
            //ex1.Test2();
            //ex1.Test3();
            //ex1.Test4();
            ex1.Test5();
            ex1.Test6();
        }
    }
}
