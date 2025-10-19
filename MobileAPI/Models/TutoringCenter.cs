using System.Collections.Generic;

namespace MobileAPI.Models
{
    public class TutoringCenter : EntityBase
    {
        public TutoringCenter()
        {
            Hours = new List<HoursOfOperation>();
            School = new School();
        }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public List<HoursOfOperation> Hours { get; set; }
    }
}