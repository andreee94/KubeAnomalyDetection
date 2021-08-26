using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Core.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Context;

namespace AnomalyDetection.Manager
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // EnsureDbCreation(host.Services);

            await ReloadTrainingJobs(host.Services).ConfigureAwait(false);

            host.Run();
        }

        // private static void EnsureDbCreation(IServiceProvider provider)
        // {
        //     using (var serviceScope = provider.CreateScope())
        //     using (var db = serviceScope.ServiceProvider.GetService<ManagerContext>())
        //     {
        //         db.Database.EnsureCreated();
        //     }
        // }

        private static async Task ReloadTrainingJobs(IServiceProvider provider)
        {
            using (var serviceScope = provider.CreateScope())
            using (var service = serviceScope.ServiceProvider.GetService<IReloadTrainingJobsService>())
            {
                await service.Run().ConfigureAwait(false);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
