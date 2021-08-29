using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using AnomalyDetection.Data.Model.Db;
using Microsoft.EntityFrameworkCore;

namespace AnomalyDetection.Data.Model.Db
{
    [Index(nameof(Name), IsUnique = true)]
    public class DbMetric : DbCrudModel
    {

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        public string Query { get; set; }

        [Required]
        [MaxLength(16)]
        public string TrainingSchedule { get; set; }

        [ForeignKey("DatasourceId")]
        public DbDatasource Datasource { get; set; }

        // [Required]
        public int DatasourceId { get; set; }


        // public string Hash()
        // {
        //     StringBuilder sb = new();
        //     sb.Append(Name);
        //     sb.Append(Query);
        //     sb.Append(Datasource.Hash());
        //     sb.Append(TrainingSchedule);

        //     MD5 md5 = new MD5CryptoServiceProvider();
        //     byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(sb.ToString()));
        //     return BitConverter.ToString(hashBytes);
        // }
    }
}