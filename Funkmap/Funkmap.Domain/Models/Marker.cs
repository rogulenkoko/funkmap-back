using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class Marker
    {
        public string Login { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public EntityType ModelType { get; set; }
        public Instruments Instrument { get; set; }
        public bool IsPriority { get; set; }
    }
}
