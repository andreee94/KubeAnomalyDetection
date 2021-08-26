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
    public abstract partial class CrudControllerTests<T> where T : CrudModel
    {
        protected readonly Random _random = new();
        protected readonly Mock<ICrudRepository<T>> _repositoryStub = new();
        protected readonly Mock<ILogger<CrudController<T>>> _loggerStub = new();

        protected virtual T CreateRandomItem()
        {
            return new CrudModel()
            {
                Id = _random.Next(1000)
            } as T;
        }
    }
}
