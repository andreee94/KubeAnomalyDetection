
using AnomalyDetection.Data.Model.Api;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public class MetricControllerTests : CrudControllerTests<ApiMetric>
    {
        protected override ApiMetric CreateRandomItem()
        {
            return new Filler<ApiMetric>().Create();

            // Datasource datasource = new()
            // {
            //     Id = _random.Next(1000000),
            //     Url = "url",
            //     IsAuthenticated = false,
            //     DatasourceType = "type"
            // };

            // return new ApiMetric
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