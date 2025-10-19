using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class SchoolEventRepo : RepoBase<SchoolEvent>
    {
        public SchoolEventRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}