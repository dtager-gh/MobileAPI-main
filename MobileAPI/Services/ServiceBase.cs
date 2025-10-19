using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public abstract class ServiceBase<T> where T : EntityBase, new()
    {
        protected readonly IRepo<T> repo;
        protected readonly ILogger log;
        protected T entity { get; set; }
        protected List<T> entities { get; set; }

        protected ServiceBase(IRepo<T> repo, ILoggerFactory logFactory)
        {
            this.repo = repo;

            log = logFactory.CreateLogger(GetType());

            entities = new List<T>();
        }

        public T? Find(int id)
        {
            return repo.Find(id);
        }

        public async Task<T?> FindAsync(int id)
        {
            return await repo.FindAsync(id);
        }

        public List<T> GetAll()
        {
            try
            {
                entities = repo.Search(x => !x.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error getting entities.");
            }

            return entities;
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                entities = await repo.Search(x => !x.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error getting entities.");
            }

            return entities;
        }

        public int Add(T entity)
        {
            int result = 0;
            try
            {
                result = repo.Add(entity);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error adding entity");
            }

            return result;
        }

        public async Task<int> AddAsync(T entity)
        {
            int result = 0;
            try
            {
                result = await repo.AddAsync(entity);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error adding entity");
            }

            return result;
        }

        public int AddRange(IEnumerable<T> entities)
        {
            int result = 0;
            try
            {
                result = repo.AddRange(entities);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error adding entities");
            }

            return result;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            int result = 0;
            try
            {
                result = await repo.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error adding entities");
            }

            return result;
        }

        public int Update(T entity)
        {
            int result = 0;

            try
            {
                result = repo.Update(entity);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error updating entity");
            }

            return result;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            int result = 0;
            try
            {
                result = await repo.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error updating entity");
            }

            return result;
        }

        public int UpdateRange(IEnumerable<T> entities)
        {
            int result = 0;
            try
            {
                result = repo.UpdateRange(entities);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error updating entities");
            }

            return result;
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<T> entities)
        {
            int result = 0;
            try
            {
                result = await repo.UpdateRangeAsync(entities);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error updating entities");
            }

            return result;
        }

        public List<T> GetRecycleBin()
        {
            try
            {
                entities = repo.Search(x => x.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error getting deleted entities.");
            }

            return entities;
        }

        public void Remove(T entity)
        {
            try
            {
                repo.Delete(entity);
            }
            catch (Exception ex)
            {
                String message = $"Error removing entity {entity.Id}.";
                log.LogError(ex, message);
            }
        }

        public async Task RemoveAsync(T entity)
        {
            try
            {
                await repo.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                String message = $"Error removing entity {entity.Id}.";
                log.LogError(ex, message);
            }
        }

        public void Delete(T entity)
        {
            if (!entity.IsDeleted)
            {
                entity.IsDeleted = true;

                try
                {
                    repo.Update(entity);
                    repo.SaveChanges();
                }
                catch (Exception ex)
                {
                    String message = $"Error soft deleting entity {entity.Id}.";
                    log.LogError(ex, message);
                }
            }
        }

        public async Task DeleteAsync(T entity)
        {
            if (!entity.IsDeleted)
            {
                entity.IsDeleted = true;

                try
                {
                    // This auto-saves asynchronously
                    await repo.UpdateAsync(entity);
                }
                catch (Exception ex)
                {
                    String message = $"Error soft deleting entity {entity.Id}.";
                    log.LogError(ex, message);
                }
            }
        }

        public int DeleteRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                if (!entity.IsDeleted)
                {
                    entity.IsDeleted = true;
                }
            }

            return UpdateRange(entities);
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                if (!entity.IsDeleted)
                {
                    entity.IsDeleted = true;
                }
            }

            return await UpdateRangeAsync(entities);
        }

        public void UnDelete(T entity)
        {
            if (entity.IsDeleted)
            {
                entity.IsDeleted = false;
                try
                {
                    repo.Update(entity);
                }
                catch (Exception ex)
                {
                    String message = $"Error restoring {entity.Id}.";
                    log.LogError(ex, message);
                }
            }
        }

        public async Task UnDeleteAsync(T entity)
        {
            if (entity.IsDeleted)
            {
                entity.IsDeleted = false;
                try
                {
                    await repo.UpdateAsync(entity);
                }
                catch (Exception ex)
                {
                    String message = $"Error restoring entity {entity.Id}.";
                    log.LogError(ex, message);
                }
            }
        }

        public void UnDelete(int id)
        {
            entity = repo.Search(x => id == x.Id).First();

            UnDelete(entity);
        }

        public async Task UnDeleteAsync(int id)
        {
            entity = repo.Search(x => id == x.Id).First();

            await UnDeleteAsync(entity);
        }
    }
}