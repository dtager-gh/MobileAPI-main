using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MobileAPI.Repos
{
    public interface IRepo<T>
    {
        int Count { get; }
        bool HasChanges { get; }
        T Find(int? id);
        Task<T?> FindAsync(int? id); // I ADDED THIS IN - NOT PART OF ORIGINAL CODE
        IEnumerable<T> GetRange(int skip, int take);
        IQueryable<T> Search(Expression<Func<T, bool>> filter = null, IOrderedQueryable<T> orderBy = null);
        int Add(T entity, bool persist = true);
        Task<int> AddAsync(T entity, bool persist = true);
        int AddRange(IEnumerable<T> entities, bool persist = true);
        Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = true);
        int Update(T entity, bool persist = true);
        Task<int> UpdateAsync(T entity, bool persist = true);
        int UpdateRange(IEnumerable<T> entities, bool persist = true);
        Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = true);
        int Delete(T entity, bool persist = true);
        int Delete(int id, long timestamp, bool persist = true);
        Task<int> DeleteAsync(T entity, bool persist = true);
        Task<int> DeleteAsync(int id, long timestamp, bool persist = true);
        int DeleteRange(IEnumerable<T> entities, bool persist = true);
        Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = true);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}