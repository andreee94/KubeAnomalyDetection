using System;
using System.Threading.Tasks;

using AnomalyDetection.Manager.Acceptance.Tests.Broker;
using FluentAssertions;
using Tynamix.ObjectFiller;
using Xunit;
using RESTFulSense.Exceptions;
using AnomalyDetection.Data.Model.Api;
using AutoMapper;
using AnomalyDetection.Data.Model.Db;

namespace AnomalyDetection.Manager.Acceptance.Tests.Api
{
    public partial class DatasourceApiTests
    {
        [Fact]
        public async Task GetSecureDatasourceById_WithHidePasswordTrue_ShouldReturnNullPassword()
        {
            //Given
            var datasource = CreateRandomDatasource();
            var hidePassword = true;

            //When
            var addedDatasource = await _broker.AddDatasourceAsync(datasource).ConfigureAwait(false);
            var retrievedDatasource = await _broker.GetSecureDatasourceByIdAsync((int)addedDatasource.Id, hidePassword).ConfigureAwait(false);

            //Then
            hidePassword.Should().BeTrue();
            retrievedDatasource.Should().NotBeNull();
            retrievedDatasource.Should().BeEquivalentTo(datasource, options =>
            {
                options.Excluding(d => d.Id);
                options.Excluding(d => d.Password);
                return options;
            });
            retrievedDatasource.Password.Should().BeNull();
        }

        [Fact]
        public async Task GetSecureDatasourceById_WithHidePasswordFalse_ShouldReturnNotNullPassword()
        {
            //Given
            var datasource = CreateRandomDatasource();
            var hidePassword = false;

            //When
            var addedDatasource = await _broker.AddDatasourceAsync(datasource).ConfigureAwait(false);
            var retrievedDatasource = await _broker.GetSecureDatasourceByIdAsync((int)addedDatasource.Id, hidePassword).ConfigureAwait(false);

            //Then
            hidePassword.Should().BeFalse();
            retrievedDatasource.Should().NotBeNull();
            retrievedDatasource.Password.Should().NotBeNull();
            retrievedDatasource.Should().BeEquivalentTo(datasource, options =>
            {
                options.Excluding(d => d.Id);
                return options;
            });
            retrievedDatasource.Password.Should().Be(datasource.Password);
        }

        [Fact]
        public async Task AddMetric_ShouldHideDatasourcePassword()
        {
            //Given
            var datasource = CreateRandomItem<ApiDatasource>();
            var metric = CreateRandomItem<ApiMetric>();

            //When
            var addedDatasource = await _broker.AddDatasourceAsync(datasource).ConfigureAwait(false);
            metric.Datasource = addedDatasource;
            metric.Datasource.Password = null; // we  should not pass the datasource password when adding a metric
            var addedMetric = await _broker.AddMetricAsync(metric).ConfigureAwait(false);

            //Then
            addedDatasource.Should().NotBeNull();
            addedMetric.Should().NotBeNull();
            addedMetric.Datasource.Should().NotBeNull();
            addedMetric.Datasource.Password.Should().BeNull();
        }

        [Fact]
        public void Automapper_WithBasic_ShouldWork()
        {
            //Given
            var apiDatasource = CreateRandomItem<ApiDatasource>();
            var apiMetric = CreateRandomItem<ApiMetric>();
            var apiTrainingJob = CreateRandomItem<ApiTrainingJob>();

            //When
            var dbDatasource = _broker.Mapper.Map<DbDatasource>(apiDatasource);
            var dbMetric = _broker.Mapper.Map<DbMetric>(apiMetric);
            var dbTrainingJob = _broker.Mapper.Map<DbTrainingJob>(apiTrainingJob);

            //Then
            dbDatasource.Should().BeEquivalentTo<ApiDatasource>(apiDatasource);
            dbMetric.Should().BeEquivalentTo<ApiMetric>(apiMetric, options => options.Excluding(a => a.Datasource));
            dbTrainingJob.Should().BeEquivalentTo<ApiTrainingJob>(apiTrainingJob, options => options.Excluding(a => a.Metric.Datasource));
        }

        [Fact]
        public void AutomapperReversed_WithBasic_ShouldWork()
        {
            //Given
            var dbDatasource = CreateRandomItem<DbDatasource>();
            var dbMetric = CreateRandomItem<DbMetric>();
            var dbTrainingJob = CreateRandomItem<DbTrainingJob>();

            //When
            var apiDatasource = _broker.Mapper.Map<ApiDatasource>(dbDatasource);
            var apiMetric = _broker.Mapper.Map<ApiMetric>(dbMetric);
            var apiTrainingJob = _broker.Mapper.Map<ApiTrainingJob>(dbTrainingJob);

            //Then
            dbDatasource.Should().BeEquivalentTo(apiDatasource, options => options.Excluding(d => d.Password));
            dbMetric.Should().BeEquivalentTo<ApiMetric>(apiMetric, options =>
            {
                options.Excluding(a => a.Id);
                options.Excluding(a => a.Datasource.Password);
                return options;
            });
            dbTrainingJob.Should().BeEquivalentTo<ApiTrainingJob>(apiTrainingJob, options =>
            {
                options.Excluding(a => a.Id);
                options.Excluding(a => a.Metric.Datasource.Password);
                return options;
            });

            apiDatasource.Password.Should().BeNull();
            apiMetric.Datasource.Password.Should().BeNull();
        }

        [Fact]
        public async Task InsertDatasourceAndMetric_ShouldSucceed()
        {
            //Given
            var datasource1 = CreateRandomItem<ApiDatasource>();
            var datasource2 = CreateRandomItem<ApiDatasource>();
            var datasource3 = CreateRandomItem<ApiDatasource>();
            var metric1 = CreateRandomItem<ApiMetric>();
            var metric2 = CreateRandomItem<ApiMetric>();
            var metric3 = CreateRandomItem<ApiMetric>();

            //When
            var addedDatasource1 = await _broker.AddDatasourceAsync(datasource1).ConfigureAwait(false);
            metric1.Datasource = addedDatasource1;
            var addedDatasource2 = await _broker.AddDatasourceAsync(datasource2).ConfigureAwait(false);
            metric2.Datasource = addedDatasource2; // 2 metrics from the same datasource
            var addedDatasource3 = await _broker.AddDatasourceAsync(datasource3).ConfigureAwait(false);
            metric3.Datasource = addedDatasource3;

            var addedMetric1 = await _broker.AddMetricAsync(metric1).ConfigureAwait(false);
            var addedMetric2 = await _broker.AddMetricAsync(metric2).ConfigureAwait(false);
            var addedMetric3 = await _broker.AddMetricAsync(metric3).ConfigureAwait(false);

            var allDatasources = await _broker.GetAllDatasourcesAsync().ConfigureAwait(false);
            var allMetrics = await _broker.GetAllMetricsAsync().ConfigureAwait(false);

            var fetchedMetric1 = await _broker.GetMetricByIdAsync((int)addedMetric1.Id).ConfigureAwait(false);
            var fetchedMetric2 = await _broker.GetMetricByIdAsync((int)addedMetric2.Id).ConfigureAwait(false);
            var fetchedMetric3 = await _broker.GetMetricByIdAsync((int)addedMetric3.Id).ConfigureAwait(false);

            var datasources = await _broker.GetAllDatasourcesAsync().ConfigureAwait(false);
            var metrics = await _broker.GetAllMetricsAsync().ConfigureAwait(false);

            //Then
            addedDatasource1.Should().BeEquivalentTo(datasource1, options => options.Excluding(d => d.Id));
            addedDatasource2.Should().BeEquivalentTo(datasource2, options => options.Excluding(d => d.Id));
            addedDatasource3.Should().BeEquivalentTo(datasource3, options => options.Excluding(d => d.Id));
            addedMetric1.Should().BeEquivalentTo(metric1, options =>
            {
                options.Excluding(m => m.Id);
                options.Excluding(m => m.Datasource.Password);
                return options;
            });
            addedMetric2.Should().BeEquivalentTo(metric2, options =>
            {
                options.Excluding(m => m.Id);
                options.Excluding(m => m.Datasource.Password);
                return options;
            });
            addedMetric3.Should().BeEquivalentTo(metric3, options =>
            {
                options.Excluding(m => m.Id);
                options.Excluding(m => m.Datasource.Password);
                return options;
            });

            fetchedMetric1.Should().BeEquivalentTo(addedMetric1, options =>
            {
                options.Excluding(m => m.Id);
                options.Excluding(m => m.Datasource.Password);
                return options;
            });
            fetchedMetric2.Should().BeEquivalentTo(addedMetric2, options =>
            {
                options.Excluding(m => m.Id);
                options.Excluding(m => m.Datasource.Password);
                return options;
            });
            fetchedMetric3.Should().BeEquivalentTo(addedMetric3, options =>
            {
                options.Excluding(m => m.Id);
                options.Excluding(m => m.Datasource.Password);
                return options;
            });
        }

        [Fact]
        public async Task ShouldCreateFetchAndDeleteADatasource()
        {
            //Given
            var inputDatasource = CreateRandomDatasource();

            //When
            var addedDatasource = await _broker.AddDatasourceAsync(inputDatasource).ConfigureAwait(false);
            var fetchedDatasource = await _broker.GetDatasourceByIdAsync((int)addedDatasource.Id).ConfigureAwait(false);

            await _broker.DeleteDatasourceAsync((int)addedDatasource.Id).ConfigureAwait(false);
            Func<Task> deleteAction = async () => { var fetchedDatasourceAfterDelete = await _broker.GetDatasourceByIdAsync((int)addedDatasource.Id).ConfigureAwait(false); };
            var allDatasources = await _broker.GetAllDatasourcesAsync().ConfigureAwait(false);

            //Then
            addedDatasource.Should().BeEquivalentTo(inputDatasource,
                                        options => options.ComparingByMembers<ApiDatasource>()
                                                          .Excluding(item => item.Id));
            fetchedDatasource.Should().BeEquivalentTo(addedDatasource, options => options.Excluding(d => d.Password));
            await deleteAction.Should().ThrowAsync<HttpResponseNotFoundException>().ConfigureAwait(false);
            allDatasources?.Should().NotContain(addedDatasource);
        }

        [Fact]
        public async Task GetItem_WithWrongId_ShouldReturnNotFound()
        {
            //Given
            var id = _random.Next(1000000);

            //When
            Func<Task> getAction = async () => await _broker.GetDatasourceByIdAsync(id).ConfigureAwait(false);

            //Then
            await getAction.Should().ThrowAsync<HttpResponseNotFoundException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task DeleteItem_WithWrongId_ShouldReturnNotFound()
        {
            //Given
            var id = _random.Next(1000000);

            //When
            Func<Task> deleteAction = async () => await _broker.DeleteDatasourceAsync(id).ConfigureAwait(false);

            //Then
            await deleteAction.Should().ThrowAsync<HttpResponseNotFoundException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task EditItem_WithWrongId_ShouldReturnNotFound()
        {
            //Given
            var item = CreateRandomDatasource();
            var id = _random.Next(1000000);

            //When
            Func<Task> editAction = async () => await _broker.EditDatasourceAsync(id, item).ConfigureAwait(false);

            //Then
            await editAction.Should().ThrowAsync<HttpResponseNotFoundException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task EditNullItem_ShouldReturnBadRequestAsync()
        {
            //Given
            var item = (ApiDatasource)null;
            var id = _random.Next(1000000);

            //When
            Func<Task> addAction = async () => await _broker.EditDatasourceAsync(id, item).ConfigureAwait(false);

            //Then
            await addAction.Should().ThrowAsync<HttpResponseBadRequestException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task AddNullItem_ShouldReturnBadRequestAsync()
        {
            //Given

            //When
            Func<Task> addAction = async () => await _broker.AddDatasourceAsync(null).ConfigureAwait(false);

            //Then
            await addAction.Should().ThrowAsync<HttpResponseBadRequestException>().ConfigureAwait(false);
        }
    }
}