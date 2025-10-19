using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class UserProfileRepo : RepoBase<UserProfile>
    {
        public UserProfileRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}