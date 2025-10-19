using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class SchoolRepo : RepoBase<School>
    {
        public SchoolRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}