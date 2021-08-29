using System.Linq;
using AnomalyDetection.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Manager.Acceptance.Tests.Broker
{
    public class InMemorySqliteWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private SqliteConnection Connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();

            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ManagerContext>));

                services.Remove(descriptor);

                services.AddDbContext<ManagerContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlite(Connection);
                    // options.UseSqlite("Filename=:memory:");
                    // options.UseInMemoryDatabase("InMemoryDbForTesting");
                    _ = options;
                });

                // services.AddAutoMapper(typeof(Startup));

                // var sp = services.BuildServiceProvider();
                // using var scope = sp.CreateScope();
                // var scopedServices = scope.ServiceProvider;
                // var db = scopedServices.GetRequiredService<ManagerContext>();
                // var logger = scopedServices.GetRequiredService<ILogger<InMemorySqliteWebApplicationFactory<TStartup>>>();

                // db.Database.EnsureDeleted();
                // db.Database.EnsureCreated();

                // db.Database.Migrate();

                // var a = db.Database.GetMigrations();
                // var b = db.Database.GetAppliedMigrations();

                // try
                // {
                //     Utilities.InitializeDbForTests(db);
                // }
                // catch (Exception ex)
                // {
                //     logger.LogError(ex, "An error occurred seeding the " +
                //         "database with test messages. Error: {Message}", ex.Message);
                // }
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Connection.Close();
        }
    }
}