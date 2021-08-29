using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AnomalyDetection.Data.Model.Option;
using AnomalyDetection.Data.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnomalyDetection.Core.Service
{
    public class SchedulerService
    {
        private readonly ILogger<SchedulerService> _logger;
        private readonly IMetricRepository _metricRepository;

        public SchedulerService(ILogger<SchedulerService> logger, IMetricRepository metricRepository)
        {
            _metricRepository = metricRepository;
            _logger = logger;
        }

        public async Task Loop(CancellationToken cancelToken)
        {
            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    await Task.FromCanceled(cancelToken).ConfigureAwait(false);
                    return;
                }
                // IList<Metric> metricList = await _metricRepository.GetAllAsync().ConfigureAwait(false);

                // foreach (var metric in metricList)
                // {
                    
                // }
            }
        }
    }
}