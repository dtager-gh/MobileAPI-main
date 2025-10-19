using System;
using System.Diagnostics.CodeAnalysis;

namespace MobileAPI.Models
{
    public class UserProfile : EntityBase
    {
        public UserProfile() 
        { 
            UserId = string.Empty;
        }

        [NotNull]
        public String UserId { get; set; }
    }
}