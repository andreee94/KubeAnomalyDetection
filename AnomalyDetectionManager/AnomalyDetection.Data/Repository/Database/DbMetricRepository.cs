using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Context;
using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Db;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AnomalyDetection.Data.Repository.Database
{
    public class DbMetricRepository : DbCrudRepository<ApiMetric, DbMetric>, IMetricRepository
    {
        public DbMetricRepository(IMapper mapper, ManagerContext context)
            : base(mapper, context, context.Metrics)
        {
        }

        public override async Task<IList<ApiMetric>> GetAllAsync()
        {
            var items = _dbSet.AsNoTracking()
                            .Include(item => item.Datasource)
                            .ToList();
            return _mapper.Map<IList<ApiMetric>>(items);
        }

        public async Task<ApiMetric?> GetByIdAsync(int id)
        {
            var item = _dbSet.AsNoTracking()
                            .Include(item => item.Datasource)
                            .FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(_mapper.Map<ApiMetric>(item)).ConfigureAwait(false);
        }
    }
}