using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Queue;

namespace AnomalyDetection.Core.Service.Queue
{
    public class DefaultBackgroundQueueService : IBackgroundQueueService
    {
        private readonly Channel<CrudEvent<ApiTrainingJob>> _queue;

        public DefaultBackgroundQueueService(int capacity)
        {
            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<CrudEvent<ApiTrainingJob>>(options);
        }

        public async ValueTask EnqueueAsync(CrudEvent<ApiTrainingJob> item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _queue.Writer.WriteAsync(item).ConfigureAwait(false);
        }

        public async ValueTask<CrudEvent<ApiTrainingJob>> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}