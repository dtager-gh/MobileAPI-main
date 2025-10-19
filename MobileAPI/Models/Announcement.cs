using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileAPI.Models
{
    public class Announcement : EntityBase
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Message { get; set; } = string.Empty;
        
        [NotMapped] // Using the timestamp from EntityBase
        public DateTime DatePosted { get => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).LocalDateTime; }
        
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;
    }
}