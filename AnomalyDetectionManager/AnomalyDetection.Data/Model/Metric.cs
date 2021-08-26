using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace AnomalyDetection.Data.Model
{
    public class Metric : CrudModel
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        public string Query { get; set; }

        [Required]
        public Datasource Datasource { get; set; }

        [Required]
        [MaxLength(16)]
        public string TrainingSchedule { get; set; }

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