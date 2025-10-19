using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public class Security : EntityBase
    {
        public Security() 
        { 
            Campus = new Campus();
            Hours = new List<HoursOfOperation>();
        }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }

        public List<HoursOfOperation> Hours { get; set; }

        public string OfficeLocation { get; set; } = string.Empty;
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;
        [Phone]
        public string EmergencyNumber { get; set; }
        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public List<SecurityAlert>? SecurityAlerts { get; set; }
    }
}