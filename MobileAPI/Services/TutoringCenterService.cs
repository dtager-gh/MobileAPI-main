using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class TutoringCenterService : ServiceBase<TutoringCenter>
    {
        public TutoringCenterService(IRepo<TutoringCenter> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}