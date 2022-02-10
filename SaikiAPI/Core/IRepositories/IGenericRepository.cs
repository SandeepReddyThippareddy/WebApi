using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaikiAPI.Core.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
         Task<IEnumerable<T>> All();

         Task<T> GetById(Guid id);

         Task<bool> DeleteById(Guid id);

         Task<T> Add(T entity);

         Task<bool> Upsert(T entity);
    }
}