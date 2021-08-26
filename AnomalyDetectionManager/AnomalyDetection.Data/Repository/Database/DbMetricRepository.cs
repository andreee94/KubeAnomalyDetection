using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Context;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Data.Repository.Database
{
    public class DbMetricRepository : DbCrudRepository<Metric>, IMetricRepository
    {
        public DbMetricRepository(ManagerContext context)
            : base(context, context.Metrics)
        {
        }
    }
}