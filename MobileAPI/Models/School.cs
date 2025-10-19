using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public class School : EntityBase
    {
        public School() 
        { 
            Name = string.Empty;
            SchoolAnnouncements = new List<Announcement>();
            SchoolEvents = new List<SchoolEvent>();
            SchoolNews = new List<SchoolNews>();
            Campuses = new List<Campus>();
        }

        [Required]
        public string Name { get; set; }
        
        public List<Campus> Campuses { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Url]
        public string Website { get; set; } = string.Empty;
        
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public List<Announcement> SchoolAnnouncements { get; set; }
        public List<SchoolEvent> SchoolEvents { get; set; }
        public List<SchoolNews> SchoolNews { get; set; }
    }
}