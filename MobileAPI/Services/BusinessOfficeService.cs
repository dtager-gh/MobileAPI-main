using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;

namespace MobileAPI.Services
{
    public class BusinessOfficeService : ServiceBase<BusinessOffice>
    {
        public BusinessOfficeService(IRepo<BusinessOffice> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}