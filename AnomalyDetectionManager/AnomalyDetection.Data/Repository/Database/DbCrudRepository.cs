using System;
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
    public abstract class DbCrudRepository<TApiModel, TDbModel> : ICrudRepository<TApiModel>
        where TApiModel : ApiCrudModel
        where TDbModel : DbCrudModel
    {
        protected readonly ManagerContext _context;
        protected readonly DbSet<TDbModel> _dbSet;
        protected readonly IMapper _mapper;

        protected DbCrudRepository(IMapper mapper, ManagerContext context, DbSet<TDbModel> dbSet)
        {
            _mapper = mapper;
            _context = context;
            _dbSet = dbSet;
        }

        public virtual async Task<TApiModel?> AddAsync(TApiModel item)
        {
            item.Id = 0; // let the db to generate the id
            var dbItem = _mapper.Map<TDbModel>(item);
            var addedItem = await _dbSet.AddAsync(dbItem).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            item.Id = addedItem.Entity.Id;
            return item; // TODO return item from database _dbSet.AddAsync(item)
        }

        public virtual async Task<TApiModel?> DeleteByIdAsync(int id)
        {
            var item = await GetByIdAsync(id).ConfigureAwait(false);
            if (item is not null)
            {
                var dbItem = _mapper.Map<TDbModel>(item);
                _dbSet.Attach(dbItem);
                _dbSet.Remove(dbItem);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return item;
        }

        public virtual async Task<TApiModel?> EditAsync(int id, TApiModel item)
        {
            var existingItem = await GetByIdAsync(id).ConfigureAwait(false);
            if (existingItem is not null)
            {
                _dbSet.Update(_mapper.Map<TDbModel>(item));
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return existingItem;
        }

        public virtual async Task<IList<TApiModel>> GetAllAsync()
        {
            var items = _dbSet.AsNoTracking().ToList();
            return _mapper.Map<IList<TApiModel>>(items);
        }

        public virtual async Task<TApiModel?> GetByIdAsync(int id)
        {
            var item = _dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(_mapper.Map<TApiModel>(item)).ConfigureAwait(false);
        }
    }
}