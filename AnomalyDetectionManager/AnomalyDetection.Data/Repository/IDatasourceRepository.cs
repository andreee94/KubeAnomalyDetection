using System.Threading.Tasks;
using AnomalyDetection.Data.Model.Api;

namespace AnomalyDetection.Data.Repository
{
    public interface IDatasourceRepository : ICrudRepository<ApiDatasource>
    {

        Task<ApiDatasource?> GetByIdAsync(int id, bool hidePassword);

    }
}