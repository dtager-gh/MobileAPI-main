using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public class RegistrarService : ServiceBase<Registrar>
    {
        public RegistrarService(IRepo<Registrar> repo, ILoggerFactory logFactory) : base(repo, logFactory)
        {

        }
    }
}