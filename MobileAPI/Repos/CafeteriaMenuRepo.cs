using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class CafeteriaMenuRepo : RepoBase<CafeteriaMenu>
    {
        public CafeteriaMenuRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}