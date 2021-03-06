using AnomalyDetection.Data.Model.Db;
using Microsoft.EntityFrameworkCore;

namespace AnomalyDetection.Data.Context
{
    public class ManagerContext : DbContext
    {
        public DbSet<DbDatasource> Datasources { get; set; }
        public DbSet<DbMetric> Metrics { get; set; }
        public DbSet<DbTrainingJob> TrainingJobs { get; set; }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     switch (_dbProviderOptions.DbType.ToLower())
        //     {
        //         case "sqlite":
        //             optionsBuilder.UseSqlite(_dbProviderOptions.ConnectionString);
        //             break;
        //         case "postgres":
        //             optionsBuilder.UseNpgsql(_dbProviderOptions.ConnectionString);
        //             break;
        //         default:
        //             throw new Exception($"DbProviderOptions.DbType not recognized: {_dbProviderOptions.DbType}");
        //     }

        //     // optionsBuilder.UseSqlite(_dbProviderOptions.ConnectionString);
        // }
    }
}