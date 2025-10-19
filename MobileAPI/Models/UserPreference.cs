using System.Diagnostics.CodeAnalysis;

namespace MobileAPI.Models
{
    public class UserPreference : EntityBase
    {
        public UserPreference() 
        { 
            UserId = string.Empty;
        }

        [NotNull]
        public string UserId { get; set; }
    }
}