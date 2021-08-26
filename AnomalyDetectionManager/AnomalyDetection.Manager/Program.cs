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

namespace AnomalyDetection.Manager
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.Services.GetService<IReloadTrainingJobsService>().Run().ConfigureAwait(false);

            // var trainingJobService = host.Services.GetService<TrainingJobService>();

            // var datasource = new Datasource()
            // {
            //     DatasourceType = "DatasourceType",
            //     Id = 1,
            //     IsAuthenticated = true,
            //     Password = "password",
            //     Username = "username",
            //     Url = "url"
            // };

            // var trainingJob1 = new TrainingJob()
            // {
            //     Metric = new Metric()
            //     {
            //         Datasource = datasource,
            //         Name = "metric1",
            //         Query = "metric1_total",
            //         Id = 1,
            //         TrainingSchedule = "* * * * *" // every minute
            //     },
            //     Id = 1,
            //     Status = "Unknown",
            //     ExecutionTime = DateTime.Now
            // };
            
            // var trainingJob2 = new TrainingJob()
            // {
            //     Metric = new Metric()
            //     {
            //         Datasource = datasource,
            //         Name = "metric2",
            //         Query = "metric2_total_new",
            //         Id = 2,
            //         TrainingSchedule = "* * * * *" // every minute
            //     },
            //     Id = 2,
            //     Status = "Unknown",
            //     ExecutionTime = DateTime.Now
            // };

            // await trainingJobService.RunSingleJobs(trainingJob1).ConfigureAwait(false);
            
            // await trainingJobService.RunSingleJobs(trainingJob2).ConfigureAwait(false);

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
