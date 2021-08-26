using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    public partial class AnomalyDetectionApiBroker
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly HttpClient baseClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;

        public AnomalyDetectionApiBroker()
        {
            this.webApplicationFactory = new InMemorySqliteWebApplicationFactory<Startup>();
            this.baseClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.baseClient);
        }
    }
}