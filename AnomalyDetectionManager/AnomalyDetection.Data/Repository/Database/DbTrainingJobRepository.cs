using AnomalyDetection.Data.Context;
using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Db;
using AutoMapper;

namespace AnomalyDetection.Data.Repository.Database
{
    public class DbTrainingJobRepository : DbCrudRepository<ApiTrainingJob, DbTrainingJob>, ITrainingJobRepository
    {
        public DbTrainingJobRepository(IMapper mapper, ManagerContext context)
            : base(mapper, context, context.TrainingJobs)
        {
        }
    }
}