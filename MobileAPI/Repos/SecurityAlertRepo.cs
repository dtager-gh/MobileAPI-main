using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class SecurityAlertRepo : RepoBase<SecurityAlert>
    {
        public SecurityAlertRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}