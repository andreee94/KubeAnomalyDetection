using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Context;
using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Db;
using AutoMapper;

namespace AnomalyDetection.Data.Repository.Database
{
    public class DbDatasourceRepository : DbCrudRepository<ApiDatasource, DbDatasource>, IDatasourceRepository
    {
        public DbDatasourceRepository(IMapper mapper, ManagerContext context)
            : base(mapper, context, context.Datasources)
        {
        }


        public async Task<ApiDatasource?> GetByIdAsync(int id, bool hidePassword)
        {
            if (hidePassword)
            {
                return await GetByIdAsync(id).ConfigureAwait(false);
            }
            else
            {
                var dbItem = _dbSet.FirstOrDefault(x => x.Id == id);
                var apiItem = _mapper.Map<ApiDatasource>(dbItem);
                apiItem.Password = dbItem?.Password;
                return await Task.FromResult(apiItem).ConfigureAwait(false);
            }
        }
    }
}