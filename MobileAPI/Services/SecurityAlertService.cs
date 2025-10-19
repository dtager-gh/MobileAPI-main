using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;

namespace MobileAPI.Services
{
    public class SecurityAlertService : ServiceBase<SecurityAlert>
    {
        public SecurityAlertService(IRepo<SecurityAlert> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}