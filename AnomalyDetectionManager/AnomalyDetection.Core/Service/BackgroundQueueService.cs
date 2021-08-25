using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using AnomalyDetection.Core.Service.Queue;
using AnomalyDetection.Data.Model.Queue;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Core.Service
{
    public class BackgroundQueueService : BackgroundService
    {
        private readonly ILogger<BackgroundQueueService> _logger;
        private readonly TrainingJobService _trainingJobService;
        private readonly IBackgroundQueue _queue;

        public BackgroundQueueService(ILogger<BackgroundQueueService> logger, TrainingJobService trainingJobService, IBackgroundQueue queue)
        {
            _logger = logger;
            _queue = queue;
            _trainingJobService = trainingJobService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var crudEvent = await _queue.DequeueAsync(stoppingToken).ConfigureAwait(false);

                    await ProcessCrudEvent(crudEvent).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Operation cancelled.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing task work item.");
                }
            }
        }

        public async Task ProcessCrudEvent(CrudEvent<TrainingJob> crudEvent)
        {
            switch (crudEvent.Action)
            {
                case CrudAction.Create:
                case CrudAction.Edit:
                    await _trainingJobService.CreateCronJob(crudEvent.Item).ConfigureAwait(false);
                    break;
                case CrudAction.Delete:
                    await _trainingJobService.DeleteCronJob(crudEvent.Item).ConfigureAwait(false);
                    break;
                default:
                    _logger.LogError($"Not able to process action: {crudEvent.Action}");
                    break;
            }
        }
    }
}