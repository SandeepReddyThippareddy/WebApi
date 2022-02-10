using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAPI.BLL.Core.IRepositories;
using WebAPI.BLL.Data;

namespace WebAPI.BLL.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected WebApiContext _context;
        protected DbSet<T> _dbSet;
        protected ILogger _logger;

        public GenericRepository(WebApiContext context, ILogger logger)
        {
            this._context = context;
            _dbSet = context.Set<T>();
            this._logger = logger;
        }


        public virtual async Task<T> Add(T entity)
        {
            if(entity == null){
                return null;
            };

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<bool> DeleteById(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            _dbSet.Remove(entity);

            return true;
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}