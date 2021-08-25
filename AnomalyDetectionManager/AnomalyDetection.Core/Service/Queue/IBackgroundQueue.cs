using System;
using System.Threading;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Model.Queue;

namespace AnomalyDetection.Core.Service.Queue
{
    public interface IBackgroundQueue
    {
        ValueTask QueueBackgroundCrudEventAsync(CrudEvent<TrainingJob> crudEvent);

        ValueTask<CrudEvent<TrainingJob>> DequeueAsync(CancellationToken cancellationToken);
    }

}