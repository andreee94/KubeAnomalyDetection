using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnomalyDetection.Data.Repository
{
    public interface ICrudRepository<T>
    {
        Task<IList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> AddAsync(T item);
        Task<T?> EditAsync(int id, T item);
        Task<T?> DeleteByIdAsync(int id);
    }
}