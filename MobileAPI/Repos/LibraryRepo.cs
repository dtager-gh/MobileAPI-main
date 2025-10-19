using Microsoft.Extensions.Configuration;
using MobileAPI.Models;

namespace MobileAPI.Repos
{
    public class LibraryRepo : RepoBase<Library>
    {
        public LibraryRepo(IConfiguration configuration) : base(configuration)
        {

        }
    }
}