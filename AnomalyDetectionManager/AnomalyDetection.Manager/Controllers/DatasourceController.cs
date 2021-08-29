using System.Threading.Tasks;
using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatasourceController : CrudController<ApiDatasource>
    {
        public DatasourceController(ILogger<DatasourceController> logger, IDatasourceRepository repository)
            : base(logger, repository)
        {
        }

        [HttpGet]
        [Route("/api/secure/[controller]/{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] bool hidePassword)
        {
            _logger.LogInformation($"GET: GetById({id}, {hidePassword})");

            var result = await ((_repository as IDatasourceRepository)?
                .GetByIdAsync(id, hidePassword))
                .ConfigureAwait(false);

            return result is not null ? Ok(result) : NotFound();
        }
    }
}