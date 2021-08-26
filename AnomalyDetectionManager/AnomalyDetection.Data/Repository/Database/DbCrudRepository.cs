using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Context;
using AnomalyDetection.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace AnomalyDetection.Data.Repository.Database
{
    public abstract class DbCrudRepository<T> : ICrudRepository<T> where T : CrudModel
    {
        protected readonly ManagerContext _context;
        protected readonly DbSet<T> _dbSet;

        protected DbCrudRepository(ManagerContext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public async Task<T?> AddAsync(T item)
        {
            await _dbSet.AddAsync(item).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return item; // TODO return item from database _dbSet.AddAsync(item)
        }

        public async Task<T?> DeleteByIdAsync(int id)
        {
            var item = await GetByIdAsync(id).ConfigureAwait(false);
            if (item is not null)
            {
                _dbSet.Remove(item);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return item;
        }

        public async Task<T?> EditAsync(int id, T item)
        {
            var existingItem = await GetByIdAsync(id).ConfigureAwait(false);
            if (existingItem is not null)
            {
                _dbSet.Update(item);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return existingItem;

        }

        public async Task<IList<T>> GetAllAsync()
        {
            return _dbSet.ToList();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var item = _dbSet.Find(id);
            return await Task.FromResult((T?)item).ConfigureAwait(false);
        }
    }
}