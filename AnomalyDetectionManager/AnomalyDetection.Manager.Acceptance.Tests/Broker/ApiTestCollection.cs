using Xunit;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<AnomalyDetectionApiBroker>
    {
    }
}