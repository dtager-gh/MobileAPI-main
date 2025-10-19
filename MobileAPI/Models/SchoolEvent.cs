using System;
using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public class SchoolEvent : EntityBase
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}