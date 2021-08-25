using System;
using System.Security.Cryptography;
using System.Text;

namespace AnomalyDetection.Data.Model
{
    public class TrainingJob: CrudModel
    {
        public Metric Metric { get; set; }
        public DateTime ExecutionTime { get; set; }
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