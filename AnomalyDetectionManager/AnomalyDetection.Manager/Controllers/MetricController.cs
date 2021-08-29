using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnomalyDetection.Data.Repository;
using AnomalyDetection.Data.Model.Api;

namespace AnomalyDetection.Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricController : CrudController<ApiMetric>
    {
        public MetricController(ILogger<MetricController> logger, IMetricRepository repository)
            : base(logger, repository)
        {
        }
    }
}