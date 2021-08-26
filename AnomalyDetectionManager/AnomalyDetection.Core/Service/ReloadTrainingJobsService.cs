using System.Collections.Generic;
using System.Threading.Tasks;
using AnomalyDetection.Core.Service;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Repository;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Core.Service
{
    public class ReloadTrainingJobsService : IReloadTrainingJobsService
    {
        private readonly ILogger<ReloadTrainingJobsService> _logger;
        private readonly TrainingJobService _trainingJobService;
        private readonly ITrainingJobRepository _trainingJobRepository;

        public ReloadTrainingJobsService(ILogger<ReloadTrainingJobsService> logger, TrainingJobService trainingJobService, ITrainingJobRepository trainingJobRepository)
        {
            _logger = logger;
            _trainingJobService = trainingJobService;
            _trainingJobRepository = trainingJobRepository;
        }

        public async Task Run()
        {
            _logger.LogInformation("Running StartupService");

            IList<TrainingJob> trainingJobs = await _trainingJobRepository.GetAllAsync().ConfigureAwait(false);

            foreach (var trainingJob in trainingJobs)
            {
                await _trainingJobService.CreateCronJob(trainingJob).ConfigureAwait(false);
            }
        }

        void System.IDisposable.Dispose() {}
    }
}