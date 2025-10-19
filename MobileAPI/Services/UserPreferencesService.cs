using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class UserPreferencesService : ServiceBase<UserPreference>
    {
        public UserPreferencesService(IRepo<UserPreference> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }


        public async Task<UserPreference> FindAsync(int Id, string UserId)
        {
            try
            {
                return await repo.Search(x => !x.IsDeleted &&
                                         x.UserId.Equals(UserId) &&
                                         x.Id == Id)
                                 .FirstAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<UserPreference>> GetAllAsync(string UserId)
        {
            return await repo.Search(x => !x.IsDeleted && x.UserId.Equals(UserId)).ToListAsync();
        }
    }
}