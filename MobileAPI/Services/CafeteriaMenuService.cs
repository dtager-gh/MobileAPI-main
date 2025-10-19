using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;

namespace MobileAPI.Services
{
    public class CafeteriaMenuService : ServiceBase<CafeteriaMenu>
    {
        public CafeteriaMenuService(IRepo<CafeteriaMenu> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}
