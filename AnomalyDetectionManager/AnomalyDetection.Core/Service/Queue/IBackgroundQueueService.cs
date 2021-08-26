using System;
using System.Threading;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Model.Queue;

namespace AnomalyDetection.Core.Service.Queue
{
    public interface IBackgroundQueueService
    {
        ValueTask EnqueueAsync(CrudEvent<TrainingJob> crudEvent);

        ValueTask<CrudEvent<TrainingJob>> DequeueAsync(CancellationToken cancellationToken);
    }

}