using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Repository.Mock;
using AnomalyDetection.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using k8s;
using AnomalyDetection.Core.Service;
using AnomalyDetection.Data.Model.Option;
using AnomalyDetection.Core.Service.Queue;

namespace AnomalyDetection.Manager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AnomalyDetection.Manager", Version = "v1" });
            });


            services.Configure<TrainingJobOptions>(Configuration.GetSection(nameof(TrainingJobOptions)));

            services.AddSingleton<IMetricRepository, MockMetricRepository>();
            services.AddSingleton<IDatasourceRepository, MockDatasourceRepository>();
            services.AddSingleton<ITrainingJobRepository, MockTrainingJobRepository>();
            services.AddSingleton<TrainingJobService>();
            services.AddHostedService<BackgroundQueueService>();
            services.AddSingleton<IBackgroundQueue>(new DefaultBackgroundQueue(100)); // TODO Read capacity from settings
            AddIKubernetes(services);
        }

        private void AddIKubernetes(IServiceCollection services)
        {
            var kubeConfigPath = Configuration.GetValue<string>("KubeConfigPath");
            var kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);
            services.AddSingleton<IKubernetes>(new Kubernetes(kubeConfig));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnomalyDetection.Manager API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
