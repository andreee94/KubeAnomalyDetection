using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AnomalyDetection.Data.Model.Db
{
    [Index(nameof(Name), IsUnique = true)]
    public class DbDatasource : DbCrudModel
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(16)]
        public string DatasourceType { get; set; }

        [Required]
        [MaxLength(128)]
        public string Url { get; set; }

        [MaxLength(64)]
        public string? Username { get; set; }

        [MaxLength(64)]
        public string? Password { get; set; }

        [Required]
        public bool IsAuthenticated { get; set; }

        public ICollection<DbMetric> Metrics { get; set; }
    }
}