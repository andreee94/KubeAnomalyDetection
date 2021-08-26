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
using AnomalyDetection.Data.Context;
using Microsoft.EntityFrameworkCore;
using AnomalyDetection.Data.Repository.Database;

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
            services.Configure<DbProviderOptions>(Configuration.GetSection(nameof(DbProviderOptions)));

            services.AddHostedService<BackgroundQueueService>();

            // services.AddSingleton<IDatasourceRepository, MockDatasourceRepository>();
            services.AddScoped<IDatasourceRepository, DbDatasourceRepository>();
            services.AddScoped<IMetricRepository, DbMetricRepository>();
            services.AddScoped<ITrainingJobRepository, DbTrainingJobRepository>();

            services.AddScoped<TrainingJobService>();
            services.AddScoped<IReloadTrainingJobsService, ReloadTrainingJobsService>();
            services.AddSingleton<IBackgroundQueueService>(new DefaultBackgroundQueueService(100)); // TODO Read capacity from settings

            // services.AddDbContext<ManagerContext>();
            AddIKubernetes(services);
            AddDBContext(services);
        }

        private void AddIKubernetes(IServiceCollection services)
        {
            // var kubeConfigPath = Configuration.GetValue<string>("KubeConfigPath");
            // var kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);

            KubernetesClientConfiguration kubeConfig;
            KubernetesConnectionOptions options = new();
            Configuration.GetSection(nameof(KubernetesConnectionOptions)).Bind(options);

            if (options.InCluster)
            {
                kubeConfig = KubernetesClientConfiguration.InClusterConfig();
            }
            else
            {
                kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile(options.KubeConfigPath);
            }
            services.AddSingleton<IKubernetes>(new Kubernetes(kubeConfig));
        }

        private void AddDBContext(IServiceCollection services)
        {
            DbProviderOptions options = new();
            Configuration.GetSection(nameof(DbProviderOptions)).Bind(options);

            switch (options.DbType.ToLower())
            {
                case "sqlite":
                    services.AddDbContext<ManagerContext>(o => o.UseSqlite(options.ConnectionString));
                    break;
                case "postgres":
                    services.AddDbContext<ManagerContext>(o => o.UseNpgsql(options.ConnectionString));
                    break;
                default:
                    throw new Exception($"DbProviderOptions.DbType not recognized: {options.DbType}");
            }
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

            EnsureDbCreation(app.ApplicationServices);
        }

        private static void EnsureDbCreation(IServiceProvider provider)
        {
            using var serviceScope = provider.CreateScope();
            using var db = serviceScope.ServiceProvider.GetService<ManagerContext>();
            db.Database.EnsureCreated();
        }
    }
}
