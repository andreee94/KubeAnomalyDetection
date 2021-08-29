using System.Net.Http;
using AnomalyDetection.Data.Context;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RESTFulSense.Clients;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    public partial class AnomalyDetectionApiBroker
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly HttpClient baseClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;
        private readonly IMapper mapper;

        private readonly ManagerContext _context;
        private readonly IServiceScope _scope;
        public AnomalyDetectionApiBroker()
        {
            webApplicationFactory = new InMemorySqliteWebApplicationFactory<Startup>();
            baseClient = webApplicationFactory.CreateClient();
            apiFactoryClient = new RESTFulApiFactoryClient(baseClient);
            mapper = webApplicationFactory.Services.GetService<IMapper>();

            ////////////////////////////////////////
            // Init the in memory database
            _scope = webApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ManagerContext>();
            _context.Database.EnsureCreated();
        }

        public IMapper Mapper => mapper;
    }
}