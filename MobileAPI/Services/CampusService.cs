using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class CampusService : ServiceBase<Campus>
    {
        public CampusService(IRepo<Campus> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}
