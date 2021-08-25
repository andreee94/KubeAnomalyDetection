using System;
using System.Security.Cryptography;
using System.Text;

namespace AnomalyDetection.Data.Model
{
    public class Metric: CrudModel
    {
        public string Name { get; set; }
        public string Query { get; set; }
        public Datasource Datasource { get; set; }
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