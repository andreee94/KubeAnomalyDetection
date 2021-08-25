using AnomalyDetection.Data.Model;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public class DatasourceControllerTests : CrudControllerTests<Datasource>
    {
        protected override Datasource CreateRandomItem()
        {
            return new Filler<Datasource>().Create();
            
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