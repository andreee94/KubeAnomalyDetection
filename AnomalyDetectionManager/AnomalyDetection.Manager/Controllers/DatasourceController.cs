using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatasourceController : CrudController<Datasource>
    {
        public DatasourceController(ILogger<DatasourceController> logger, IDatasourceRepository repository)
            : base(logger, repository)
        {
        }
    }
}