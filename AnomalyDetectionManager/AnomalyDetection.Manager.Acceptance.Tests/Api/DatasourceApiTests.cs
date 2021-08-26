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

        private static Datasource CreateRandomItem()
        {
            return new Filler<Datasource>().Create();
        }
    }
}