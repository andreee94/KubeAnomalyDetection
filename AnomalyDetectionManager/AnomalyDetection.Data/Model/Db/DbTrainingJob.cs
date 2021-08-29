using System;
using System.ComponentModel.DataAnnotations;

namespace AnomalyDetection.Data.Model.Db
{
    public class DbTrainingJob : DbCrudModel
    {
        [Required]
        public DbMetric Metric { get; set; }

        public DateTime CreationTime { get; set; }

        [Required]
        [MaxLength(16)]
        public string Status { get; set; }
    }
}