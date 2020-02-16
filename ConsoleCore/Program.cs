using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleCore {
    class Program {
        static void Main(string[] args) {

            //setup our DI
            var serviceProvider = new ServiceCollection()
                //.AddLogging()
                //.AddSingleton<IFooService, FooService>()
                //.AddSingleton<IBarService, BarService>()
                .BuildServiceProvider();



            var i = ReadFromAppSettings();
            var n = i.GetSection("dmlInfos").Get<IList<DmlInfoExtension>>();

            //i.GetValue<"">
            Console.WriteLine("Hello World!");
        }

        // https://poychang.github.io/dotnet-core-console-app-with-configuration/
        public static IConfigurationRoot ReadFromAppSettings() {
            var envVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            // TODO: null??
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("dmlInfos.json", false)
                // optional for appsettings.xxx.json
                //.AddJsonFile($"appsettings.{envVariable}.json", optional: true)
                .Build();
        }
    }
}