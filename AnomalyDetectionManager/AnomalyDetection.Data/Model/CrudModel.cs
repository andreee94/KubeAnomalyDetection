using System.ComponentModel.DataAnnotations;

namespace AnomalyDetection.Data.Model
{
    public class CrudModel
    {
        [Key]
        public int Id { get; set; }
    }
}