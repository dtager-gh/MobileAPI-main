using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;

namespace MobileAPI.Services
{
    public class AnnouncementService : ServiceBase<Announcement>
    {
        public AnnouncementService(IRepo<Announcement> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}