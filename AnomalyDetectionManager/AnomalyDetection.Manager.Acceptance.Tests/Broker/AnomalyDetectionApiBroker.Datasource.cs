using System.Collections.Generic;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model.Api;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    public partial class AnomalyDetectionApiBroker
    {
        private const string DatasourceRelativeUrl = "api/datasource";
        private const string SecureDatasourceRelativeUrl = "api/secure/datasource";

        public async Task<ApiDatasource> AddDatasourceAsync(ApiDatasource datasource)
            => await this.apiFactoryClient.PostContentAsync(DatasourceRelativeUrl, datasource).ConfigureAwait(false);
        public async Task<IList<ApiDatasource>> GetAllDatasourcesAsync()
            => await this.apiFactoryClient.GetContentAsync<IList<ApiDatasource>>(DatasourceRelativeUrl).ConfigureAwait(false);
        public async Task<ApiDatasource> GetDatasourceByIdAsync(int id)
            => await this.apiFactoryClient.GetContentAsync<ApiDatasource>($"{DatasourceRelativeUrl}/{id}").ConfigureAwait(false);
        public async Task<ApiDatasource> GetSecureDatasourceByIdAsync(int id, bool hidePassword)
        => await this.apiFactoryClient.GetContentAsync<ApiDatasource>($"{SecureDatasourceRelativeUrl}/{id}?hidePassword={hidePassword}").ConfigureAwait(false);
        public async Task DeleteDatasourceAsync(int id)
            => await this.apiFactoryClient.DeleteContentAsync($"{DatasourceRelativeUrl}/{id}").ConfigureAwait(false);
        public async Task EditDatasourceAsync(int id, ApiDatasource datasource)
            => await this.apiFactoryClient.PutContentAsync($"{DatasourceRelativeUrl}/{id}", datasource).ConfigureAwait(false);
    }
}