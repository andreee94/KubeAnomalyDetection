
using AnomalyDetection.Data.Model.Api;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public class DatasourceControllerTests : CrudControllerTests<ApiDatasource>
    {
        protected override ApiDatasource CreateRandomItem()
        {
            return new Filler<ApiDatasource>().Create();
            
            // return new Datasource()
            // {
                // Id = _random.Next(1000000),
                // Url = "url",
                // IsAuthenticated = false,
                // DatasourceType = "type"
            // };
        }
    }
}