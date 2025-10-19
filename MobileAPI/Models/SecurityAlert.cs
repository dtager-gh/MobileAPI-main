using System;
using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public enum AlertLevel
    {
        GREEN, YELLOW, ORANGE, RED
    }

    public class SecurityAlert : EntityBase
    {
        public SecurityAlert()
        { 
        
        }

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        public DateTime AlertDate { get; set; }
        public AlertLevel Level { get; set; }
        public bool IsActive { get; set; }
        public int SecurityId { get; set; }
        public Security Security { get; set; } = null!;
    }
}