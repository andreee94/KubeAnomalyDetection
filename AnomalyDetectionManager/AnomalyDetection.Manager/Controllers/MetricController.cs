using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Repository;
using System.Threading.Tasks;
using System;

namespace AnomalyDetection.Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricController : CrudController<Metric>
    {
        public MetricController(ILogger<MetricController> logger, IMetricRepository repository)
            : base(logger, repository)
        {
        }
    }
}