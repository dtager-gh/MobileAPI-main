using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class SecurityService : ServiceBase<Security>
    {
        public SecurityService(IRepo<Security> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}