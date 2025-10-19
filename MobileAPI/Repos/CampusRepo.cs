using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class CampusRepo : RepoBase<Campus>
    {
        public CampusRepo(IConfiguration config) : base(config)
        {

        }
    }
}
