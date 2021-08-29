using System.Threading.Tasks;
using AnomalyDetection.Core.Service.Queue;
using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Queue;
using AnomalyDetection.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingJobController  : CrudController<ApiTrainingJob>
    {
        private readonly IBackgroundQueueService _queue;

        public TrainingJobController(ILogger<TrainingJobController> logger, ITrainingJobRepository repository, IBackgroundQueueService queue)
            : base(logger, repository)
        {
            this._queue = queue;
        }
        protected override async Task ProcessCrudEvent(CrudEvent<ApiTrainingJob> crudEvent)
        {
            await _queue.EnqueueAsync(crudEvent).ConfigureAwait(false);
        }
    }
}