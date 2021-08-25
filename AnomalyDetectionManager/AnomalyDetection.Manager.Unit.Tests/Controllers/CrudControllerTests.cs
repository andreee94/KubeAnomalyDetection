using System;
using AnomalyDetection.Data.Repository;
using Xunit;
using Moq;
using AnomalyDetection.Data.Model;
using Microsoft.Extensions.Logging;
using AnomalyDetection.Manager.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public abstract class CrudControllerTests<T> where T : CrudModel
    {
        protected readonly Random _random = new();
        protected readonly Mock<ICrudRepository<T>> _repositoryStub = new();
        protected readonly Mock<ILogger<CrudController<T>>> _loggerStub = new();

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        [Fact]
        public async Task AddAsync_WithNullItem_ShouldReturnBadRequest()
        {
            // Arrange
            T item = null;

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.Add(item).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddAsync_WithNewItem_ShouldReturnCreatedResult()
        {
            // Arrange
            T item = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.AddAsync(It.IsAny<T>())).ReturnsAsync(item);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.Add(item).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();

            var createdMetric = (result as CreatedAtActionResult)?.Value as T;

            createdMetric.Should().BeEquivalentTo(item,
                options => options.ComparingByMembers<T>().Excluding(s => s.Id)
            );
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        [Fact]
        public async Task GetAll_WithEmptyList_ShouldReturnNoContent()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<T>());

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.GetAll().ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetAll_WithSomeItems_ShouldReturnAllItems()
        {
            IList<T> metricList = new List<T>() {
                CreateRandomItem(),
                CreateRandomItem(),
                CreateRandomItem()
            };

            // Arrange
            _repositoryStub.Setup(repo => repo.GetAllAsync()).ReturnsAsync(metricList);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.GetAll().ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var returnedMetricList = (result as OkObjectResult)?.Value as IList<T>;

            returnedMetricList.Should().BeEquivalentTo(metricList,
                options => options.ComparingByMembers<T>()
            );
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        [Fact]
        public async Task GetById_WithMissingId_ShouldReturnNotFound()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((T)null);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.GetById(_random.Next(100)).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnTheItem()
        {
            // Arrange
            var item = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.GetById(item.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var foundMetric = (result as OkObjectResult)?.Value as T;

            foundMetric.Should().BeEquivalentTo(item,
                options => options.ComparingByMembers<T>());
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        [Fact]
        public async Task EditById_WithMissingId_ShouldReturnNotFound()
        {
            // Arrange
            var item = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.EditAsync(It.IsAny<int>(), It.IsAny<T>()))
                .ReturnsAsync((T)null);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.EditById(_random.Next(100), item).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EditById_WithNullItem_ShouldReturnBadRequest()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.EditAsync(It.IsAny<int>(), It.IsAny<T>()))
                .ReturnsAsync((T)null);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.EditById(_random.Next(100), null).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task EditById_WithValidId_ShouldReturnUpdatedItem()
        {
            // Arrange
            var item = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.EditAsync(It.IsAny<int>(), It.IsAny<T>()))
                .ReturnsAsync(item);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.EditById(item.Id, item).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var foundMetric = (result as OkObjectResult)?.Value as T;

            foundMetric.Should().BeEquivalentTo(item,
                options => options.ComparingByMembers<T>());
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        [Fact]
        public async Task DeleteById_WithMissingId_ShouldReturnNotFound()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((T)null);

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.DeleteById(_random.Next(100)).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteById_WithValidId_ShouldReturnOk()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CreateRandomItem());

            var controller = new CrudController<T>(_loggerStub.Object, _repositoryStub.Object);

            // Act
            var result = await controller.DeleteById(_random.Next(100)).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        protected virtual T CreateRandomItem()
        {
            return new CrudModel()
            {
                Id = _random.Next(1000)
            } as T;
        }
    }
}
