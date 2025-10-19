using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class SecurityRepo : RepoBase<Security>
    {
        public SecurityRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}