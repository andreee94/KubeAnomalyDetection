using AnomalyDetection.Data.Model;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public class MetricControllerTests : CrudControllerTests<Metric>
    {
        protected override Metric CreateRandomItem()
        {
            return new Filler<Metric>().Create();

            // Datasource datasource = new()
            // {
            //     Id = _random.Next(1000000),
            //     Url = "url",
            //     IsAuthenticated = false,
            //     DatasourceType = "type"
            // };

            // return new Metric
            // {
            //     Id = _random.Next(1000000),
            //     Datasource = datasource,
            //     Query = "query",
            //     Name = "MetricName",
            //     TrainingSchedule = "@daily"
            // };
        }
    }
}