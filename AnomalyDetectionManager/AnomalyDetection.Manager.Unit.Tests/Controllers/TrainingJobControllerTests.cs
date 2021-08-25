using System;
using AnomalyDetection.Data.Model;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public class TrainingJobControllerTests : CrudControllerTests<TrainingJob>
    {
        protected override TrainingJob CreateRandomItem()
        {
            return new Filler<TrainingJob>().Create();

            // Datasource datasource = new()
            // {
            //     Id = 1,
            //     DatasourceType = "Prometheus",
            //     IsAuthenticated = false,
            //     Url = "127.0.0.1:9090/api/query"
            // };

            // Metric metric = new()
            // {
            //     Id = 1,
            //     Name = "Metric1",
            //     Query = "prometheus_metric1_total",
            //     Datasource = datasource,
            //     TrainingSchedule = "@daily"
            // };

            // return new TrainingJob()
            // {
            //     Id = 1,
            //     Metric = metric,
            //     ExecutionTime = DateTime.Now,
            //     Status = "Done"
            // };
        }
    }
}