

namespace MobileAPI.Models
{
    public class Address : EntityBase
    {
        public Address() 
        { 
        
        }

        public int? GpsCoordinateId { get; set; }
        public GpsCoordinate GpsCoordinates { get; set; }

        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }  // Optional

        public string City { get; set; }

        public string State { get; set; }  // Use 2-letter state code, e.g., "CA", "NY"
        
        public string PostalCode { get; set; }  // ZIP code that supports XXXXX & XXXXX-YYYY

        public string Country { get; set; } = "USA";  // Default to USA
    }
}
