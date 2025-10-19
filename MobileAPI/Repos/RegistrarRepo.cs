using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class RegistrarRepo : RepoBase<Registrar>
    {
        public RegistrarRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}