using System.Collections.Generic;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model.Api;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    public partial class AnomalyDetectionApiBroker
    {
        private const string MetricRelativeUrl = "api/metric";

        public async Task<ApiMetric> AddMetricAsync(ApiMetric metric)
            => await this.apiFactoryClient.PostContentAsync(MetricRelativeUrl, metric).ConfigureAwait(false);
        public async Task<IList<ApiMetric>> GetAllMetricsAsync()
            => await this.apiFactoryClient.GetContentAsync<IList<ApiMetric>>(MetricRelativeUrl).ConfigureAwait(false);
        public async Task<ApiMetric> GetMetricByIdAsync(int id)
            => await this.apiFactoryClient.GetContentAsync<ApiMetric>($"{MetricRelativeUrl}/{id}").ConfigureAwait(false);
        public async Task DeleteMetricAsync(int id)
            => await this.apiFactoryClient.DeleteContentAsync($"{MetricRelativeUrl}/{id}").ConfigureAwait(false);
        public async Task EditMetricAsync(int id, ApiMetric metric)
            => await this.apiFactoryClient.PutContentAsync<ApiMetric>($"{MetricRelativeUrl}/{id}", metric).ConfigureAwait(false);
    }
}