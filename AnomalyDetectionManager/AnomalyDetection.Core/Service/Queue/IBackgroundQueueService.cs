using System.Threading;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Queue;

namespace AnomalyDetection.Core.Service.Queue
{
    public interface IBackgroundQueueService
    {
        ValueTask EnqueueAsync(CrudEvent<ApiTrainingJob> crudEvent);

        ValueTask<CrudEvent<ApiTrainingJob>> DequeueAsync(CancellationToken cancellationToken);
    }

}