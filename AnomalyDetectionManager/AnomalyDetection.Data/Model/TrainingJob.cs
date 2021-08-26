using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace AnomalyDetection.Data.Model
{
    public class TrainingJob : CrudModel
    {
        [Required]
        public Metric Metric { get; set; }

        public DateTime CreationTime { get; set; }

        [Required]
        [MaxLength(16)]
        public string Status { get; set; }

        // public string Hash()
        // {
        //     StringBuilder sb = new();
        //     sb.Append(Metric.Hash());
        //     sb.Append(ExecutionTime);
        //     sb.Append(Status);

        //     MD5 md5 = new MD5CryptoServiceProvider();
        //     byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(sb.ToString()));
        //     return BitConverter.ToString(hashBytes);
        // }
    }
}