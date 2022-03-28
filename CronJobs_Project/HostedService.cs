using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronJob_Project
{
    class HostedService : IHostedService
    {
        private readonly IConfiguration _configuration;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            do
            {
                await DelayNextExecutation(cancellationToken);
                Console.WriteLine("METODO TESTE: " + DateTime.Now);
                //GenerateFile();
            } while (!cancellationToken.IsCancellationRequested);
        }

        private async Task DelayNextExecutation(CancellationToken cancellationToken)
        {
            try
            {
                var expressinCronSchedule = _configuration["CRON_JOB_SCHEDULE"];
                var _schedule = CrontabSchedule.Parse(expressinCronSchedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
                var nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                await Task.Delay(Math.Max(0, (int)nextRun.Subtract(DateTime.Now).TotalMilliseconds), cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}, {DateTime.Now}");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            //TAS.Service.Manager.Main.GetInstance().StopAll();
            return Task.CompletedTask;
        }
    }
}