using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetection.Data.Model
{
    public class Datasource : CrudModel
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