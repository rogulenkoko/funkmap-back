using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Models
{
    public class MarkerModel
    {
        public string Login { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public EntityType ModelType { get; set; }


        public InstrumentType Instrument;
    }
}
