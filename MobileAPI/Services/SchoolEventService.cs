using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class SchoolEventService : ServiceBase<SchoolEvent>
    {
        public SchoolEventService(IRepo<SchoolEvent> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}