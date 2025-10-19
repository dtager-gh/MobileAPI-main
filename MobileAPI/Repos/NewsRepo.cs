using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class NewsRepo : RepoBase<SchoolNews>
    {
        public NewsRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}