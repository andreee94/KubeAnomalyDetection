using System;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Model.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnomalyDetection.Data.Context
{
    public class ManagerContext : DbContext
    {
        public DbSet<Datasource> Datasources { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<TrainingJob> TrainingJobs { get; set; }

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