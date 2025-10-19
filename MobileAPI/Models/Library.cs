using System.Collections.Generic;

namespace MobileAPI.Models
{
    public class Library : EntityBase
    {
        public Library() 
        {
            Hours = new List<HoursOfOperation>();
            Campus = new Campus();
        }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }

        public List<HoursOfOperation> Hours { get; set; }
    }
}