using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Core.Service.Queue;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Model.Queue;
using AnomalyDetection.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingJobController  : CrudController<TrainingJob>
    {
        private readonly IBackgroundQueueService _queue;

        public TrainingJobController(ILogger<TrainingJobController> logger, ITrainingJobRepository repository, IBackgroundQueueService queue)
            : base(logger, repository)
        {
            this._queue = queue;
        }
        protected override async Task ProcessCrudEvent(CrudEvent<TrainingJob> crudEvent)
        {
            await _queue.EnqueueAsync(crudEvent).ConfigureAwait(false);
        }
    }
}