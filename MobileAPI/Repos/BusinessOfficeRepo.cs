using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class BusinessOfficeRepo : RepoBase<BusinessOffice>
    {
        public BusinessOfficeRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}