using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class UserPreferencesRepo : RepoBase<UserPreference>
    {
        public UserPreferencesRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}