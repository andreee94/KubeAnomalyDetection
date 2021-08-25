using System.Collections.Generic;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Data.Repository
{
    public interface ITrainingJobRepository : ICrudRepository<TrainingJob>
    {
    }
}