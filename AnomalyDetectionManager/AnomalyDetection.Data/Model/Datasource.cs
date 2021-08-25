using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetection.Data.Model
{
    public class Datasource : CrudModel
    {
        public string DatasourceType { get; set; }
        public string Url { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAuthenticated { get; set; }

        // public string Hash()
        // {
        //     StringBuilder sb = new();
        //     sb.Append(DatasourceType);
        //     sb.Append(Url);
        //     sb.Append(Username);
        //     sb.Append(Password);
        //     sb.Append(IsAuthenticated);

        //     MD5 md5 = new MD5CryptoServiceProvider();
        //     byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(sb.ToString()));
        //     return BitConverter.ToString(hashBytes);
        // }
    }
}