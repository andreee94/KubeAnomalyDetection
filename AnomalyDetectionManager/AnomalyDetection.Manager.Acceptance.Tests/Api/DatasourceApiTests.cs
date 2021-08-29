using System;
using System.Threading.Tasks;

using AnomalyDetection.Manager.Acceptance.Tests.Broker;
using FluentAssertions;
using Tynamix.ObjectFiller;
using Xunit;
using RESTFulSense.Exceptions;
using AnomalyDetection.Data.Model.Api;
using AutoMapper;

namespace AnomalyDetection.Manager.Acceptance.Tests.Api
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DatasourceApiTests
    {
        private readonly AnomalyDetectionApiBroker _broker;
        private readonly Random _random;
        

        public DatasourceApiTests(AnomalyDetectionApiBroker broker)
        {
            _broker = broker;
            _random = new Random();
        }

        private static ApiDatasource CreateRandomDatasource()
        {
            return new Filler<ApiDatasource>().Create();
        }
        private static T CreateRandomItem<T>() where T : class
        {
            return new Filler<T>().Create();
        }
    }
}