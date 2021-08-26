using System.Collections.Generic;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    public partial class AnomalyDetectionApiBroker
    {
        private const string DatasourceRelativeUrl = "api/datasource";

        public async Task<Datasource> AddDatasourceAsync(Datasource datasource)
            => await this.apiFactoryClient.PostContentAsync(DatasourceRelativeUrl, datasource).ConfigureAwait(false);
        public async Task<IList<Datasource>> GetAllDatasourcesAsync()
            => await this.apiFactoryClient.GetContentAsync<IList<Datasource>>(DatasourceRelativeUrl).ConfigureAwait(false);
        public async Task<Datasource> GetDatasourceByIdAsync(int id)
            => await this.apiFactoryClient.GetContentAsync<Datasource>($"{DatasourceRelativeUrl}/{id}").ConfigureAwait(false);
        public async Task DeleteDatasourceAsync(int id)
            => await this.apiFactoryClient.DeleteContentAsync($"{DatasourceRelativeUrl}/{id}").ConfigureAwait(false);
        public async Task EditDatasourceAsync(int id, Datasource datasource)
            => await this.apiFactoryClient.PutContentAsync<Datasource>($"{DatasourceRelativeUrl}/{id}", datasource).ConfigureAwait(false);
    }
}