using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Model.Queue;

namespace AnomalyDetection.Core.Service.Queue
{
    public class DefaultBackgroundQueueService : IBackgroundQueueService
    {
        private readonly Channel<CrudEvent<TrainingJob>> _queue;

        public DefaultBackgroundQueueService(int capacity)
        {
            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<CrudEvent<TrainingJob>>(options);
        }

        public async ValueTask EnqueueAsync(CrudEvent<TrainingJob> item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _queue.Writer.WriteAsync(item).ConfigureAwait(false);
        }

        public async ValueTask<CrudEvent<TrainingJob>> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}