using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class AnnouncementRepo : RepoBase<Announcement>
    {
        public AnnouncementRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}