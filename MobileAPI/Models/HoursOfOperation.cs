using System;

namespace MobileAPI.Models
{
    public class HoursOfOperation : EntityBase
    {
        public HoursOfOperation() 
        { 
        
        }

        public DayOfWeek Day { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public int? BusinessOfficeId { get; set; }
        public BusinessOffice? BusinessOffice { get; set; }

        public int? LibraryId { get; set; }
        public Library? Library { get; set; }
    }
}
