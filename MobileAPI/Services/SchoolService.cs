using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class SchoolService : ServiceBase<School>
    {
        public SchoolService(IRepo<School> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}