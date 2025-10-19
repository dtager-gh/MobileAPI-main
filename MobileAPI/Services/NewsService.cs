using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;

namespace MobileAPI.Services
{
    public class NewsService : ServiceBase<SchoolNews>
    {
        public NewsService(IRepo<SchoolNews> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}