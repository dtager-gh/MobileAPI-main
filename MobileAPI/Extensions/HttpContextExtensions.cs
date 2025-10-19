using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace MobileAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static UserClaims GetUserClaims(this HttpContext context)
        {
            var claims = context.User.Claims;

            var userClaims = new UserClaims
            {
                UserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                EmailVerified = claims.FirstOrDefault(c => c.Type == "email_verified")?.Value == "true",
                Username = claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value,
                Nickname = claims.FirstOrDefault(c => c.Type == "nickname")?.Value,
                Groups = claims.Where(c => c.Type == "groups").Select(c => c.Value).ToArray(),
            };

            return userClaims;
        }
    }
}