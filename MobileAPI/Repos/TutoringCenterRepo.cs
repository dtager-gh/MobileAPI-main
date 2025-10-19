using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class TutoringCenterRepo : RepoBase<TutoringCenter>
    {
        public TutoringCenterRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}