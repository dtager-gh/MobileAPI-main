using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using MobileAPI.Data;
using MobileAPI.Models;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MobileAPI.Repos
{
    public abstract class RepoBase<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        protected readonly ApplicationDbContext Db;
        protected DbSet<T> Table;
        public ApplicationDbContext Context => Db;

        protected RepoBase(IConfiguration config)
        {
            DbContextOptionsBuilder<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            Db = new ApplicationDbContext(options.Options, config);
            Table = Db.Set<T>();
        }

        protected RepoBase(DbContextOptions<ApplicationDbContext> options, IConfiguration config)
        {
            Db = new ApplicationDbContext(options, config);
            Table = Db.Set<T>();
        }

        public bool HasChanges => Db.ChangeTracker.HasChanges();

        public int Count => Table.Count();

        public virtual T Find(int? id)
        {
            return Search(x => x.Id == id).First();
        }

        public async Task<T?> FindAsync(int? id)
        {
            return await Db.Set<T>().FindAsync(id);
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> filter = null, IOrderedQueryable<T> orderBy = null)
        {
            IQueryable<T> query = Table.Where(x => x.Id > 0).AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return query.OrderBy(q => orderBy);
            }

            return query;
        }

        internal static IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        public virtual IEnumerable<T> GetRange(int skip, int take)
            => RepoBase<T>.GetRange(Table, skip, take);

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> AddAsync(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> UpdateAsync(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> DeleteAsync(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual int Delete(int id, long timestamp, bool persist = true)
        {
            var entry = GetEntryFromChangeTracker(id);
            if (entry != null)
            {
                if (timestamp != null && entry.Timestamp.Equals(timestamp))
                {
                    return Delete(entry, persist);
                }
                throw new DBConcurrencyException("Unable to delete due to concurrency violation.");
            }
            Db.Entry(new T { Id = id, Timestamp = timestamp }).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> DeleteAsync(int id, long timestamp, bool persist = true)
        {
            var entry = GetEntryFromChangeTracker(id);
            if (entry != null)
            {
                if (timestamp != null && entry.Timestamp.Equals(timestamp))
                {
                    return await DeleteAsync(entry, persist);
                }
                throw new DBConcurrencyException("Unable to delete due to concurrency violation.");
            }
            Db.Entry(new T { Id = id, Timestamp = timestamp }).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? await SaveChangesAsync() : 0;
        }

        internal T GetEntryFromChangeTracker(int? id)
        {
            return Db.ChangeTracker.Entries<T>()
                .Select((EntityEntry e) => (T)e.Entity)
                    .FirstOrDefault(x => x.Id == id);
        }

        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
            }
            Db.Dispose();
            _disposed = true;
        }
    }
}