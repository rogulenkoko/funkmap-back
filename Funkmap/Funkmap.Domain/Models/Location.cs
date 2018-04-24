
namespace Funkmap.Domain.Models
{
    public class Location
    {
        public Location()
        {

        }
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override bool Equals(object obj)
        {
            var location = obj as Location;

            if (location == null) return false;

            return location.Latitude == Latitude && location.Longitude == Longitude;
        }
    }
}
