namespace MobileAPI.Models
{
    public class GpsCoordinate : EntityBase
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GpsCoordinate() 
        {
        
        }

        // Needs validation
        public GpsCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"{Latitude}, {Longitude}";
        }
    }
}
