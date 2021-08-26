using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Data.Repository.Mock
{
    public class MockTrainingJobRepository : ITrainingJobRepository
    {
        private readonly IList<TrainingJob> _trainingJobList;

        private readonly Random _random;

        public MockTrainingJobRepository()
        {
            _random = new Random();

            Datasource prometheus = new()
            {
                Id = 1,
                DatasourceType = "Prometheus",
                IsAuthenticated = false,
                Url = "127.0.0.1:9090/api/query"
            };

            Metric metric1 = new()
            {
                Id = 1,
                Name = "Metric1",
                Query = "prometheus_metric1_total",
                Datasource = prometheus,
                TrainingSchedule = "* * * * *"
            };

            Metric metric2 = new()
            {
                Id = 1,
                Name = "Metric2",
                Query = "prometheus_metric2_total",
                Datasource = prometheus,
                TrainingSchedule = "8 * * * *"
            };

            _trainingJobList = new List<TrainingJob> {
                new()
                {
                    Id = 1,
                    Metric = metric1,
                    ExecutionTime = DateTime.Now,
                    Status = "Done"
                },
                new()
                {
                    Id = 2,
                    Metric = metric2,
                    ExecutionTime = DateTime.Now,
                    Status = "Running"
                }
            };
        }

        public Task<TrainingJob?> AddAsync(TrainingJob trainingJob)
        {
            trainingJob.Id = _random.Next(1000);
            _trainingJobList.Add(trainingJob);

            return Task.FromResult((TrainingJob?)trainingJob);
        }

        public Task<TrainingJob?> DeleteByIdAsync(int id)
        {
            TrainingJob? trainingJob = _trainingJobList.FirstOrDefault(item => item.Id == id);

            if (trainingJob is not null)
            {
                _trainingJobList.Remove(trainingJob);
                return Task.FromResult((TrainingJob?)trainingJob);
            }

            return Task.FromResult((TrainingJob?)null);
        }

        public Task<TrainingJob?> EditAsync(int id, TrainingJob newTrainingJob)
        {
            TrainingJob? trainingJob = _trainingJobList.FirstOrDefault(item => item.Id == id);

            if (trainingJob is not null)
            {
                trainingJob.Metric = newTrainingJob.Metric;
                trainingJob.ExecutionTime = newTrainingJob.ExecutionTime;
                trainingJob.Status = newTrainingJob.Status;
                
                return Task.FromResult((TrainingJob?)trainingJob);
            }
            return Task.FromResult((TrainingJob?)null);
        }

        public Task<IList<TrainingJob>> GetAllAsync()
        {
            return Task.FromResult(_trainingJobList);
        }

        public Task<TrainingJob?> GetByIdAsync(int id)
        {
            return Task.FromResult(_trainingJobList.FirstOrDefault(item => item.Id == id));
        }
    }
}