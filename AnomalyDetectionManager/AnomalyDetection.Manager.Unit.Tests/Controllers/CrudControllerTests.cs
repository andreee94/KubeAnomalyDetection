using System;
using AnomalyDetection.Data.Repository;
using Moq;
using Microsoft.Extensions.Logging;
using AnomalyDetection.Manager.Controllers;
using AnomalyDetection.Data.Model.Api;

namespace AnomalyDetection.Manager.Unit.Tests.Controllers
{
    public abstract partial class CrudControllerTests<T> where T : ApiCrudModel
    {
        protected readonly Random _random = new();
        protected readonly Mock<ICrudRepository<T>> _repositoryStub = new();
        protected readonly Mock<ILogger<CrudController<T>>> _loggerStub = new();

        protected virtual T CreateRandomItem()
        {
            return new ApiCrudModel()
            {
                Id = _random.Next(1000)
            } as T;
        }
    }
}
