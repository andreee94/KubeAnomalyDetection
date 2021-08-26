using System;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Manager.Acceptance.Tests.Broker;
using FluentAssertions;
using Tynamix.ObjectFiller;
using Xunit;
using RESTFulSense.Exceptions;

namespace AnomalyDetection.Manager.Acceptance.Tests.Api
{
    public partial class DatasourceApiTests
    {
        [Fact]
        public async Task ShouldCreateFetchAndDeleteADatasource()
        {
            //Given
            var inputDatasource = CreateRandomItem();

            //When
            var addedDatasource = await _broker.AddDatasourceAsync(inputDatasource).ConfigureAwait(false);
            var fetchedDatasource = await _broker.GetDatasourceByIdAsync(addedDatasource.Id).ConfigureAwait(false);
            await _broker.DeleteDatasourceAsync(addedDatasource.Id).ConfigureAwait(false);
            Func<Task> deleteAction = async () => { var fetchedDatasourceAfterDelete = await _broker.GetDatasourceByIdAsync(addedDatasource.Id).ConfigureAwait(false); };
            var allDatasources = await _broker.GetAllDatasourcesAsync().ConfigureAwait(false);

            //Then
            addedDatasource.Should().BeEquivalentTo(inputDatasource,
                                        options => options.ComparingByMembers<Datasource>()
                                                          .Excluding(item => item.Id));
            fetchedDatasource.Should().BeEquivalentTo(addedDatasource);
            await deleteAction.Should().ThrowAsync<HttpResponseNotFoundException>().ConfigureAwait(false);
            allDatasources.Should().NotContain(addedDatasource);
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
            var item = CreateRandomItem();
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
            var item = (Datasource)null;
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