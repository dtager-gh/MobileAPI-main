using System.Collections.Generic;

namespace MobileAPI.Models
{
    public class Registrar : EntityBase
    {
        public Registrar() 
        {
            Hours = new List<HoursOfOperation>();
            Campus = new Campus();
        }

        public List<HoursOfOperation> Hours { get; set; }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }
    }
}