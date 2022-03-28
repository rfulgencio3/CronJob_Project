using CronJob_Project;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CronJobs_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            hostBuilder.RunConsoleAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                 .ConfigureServices((context, services) => { services.AddHostedService<HostedService>(); })
                 .ConfigureAppConfiguration(configuration =>
                 {
                     configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                     configuration.AddJsonFile(
                         $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                         optional: true);
                     configuration.AddEnvironmentVariables();
                 });

            return hostBuilder;
        }
    }
}
