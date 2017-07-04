using Funkmap.Common;
using Funkmap.Data.Entities;

namespace Funkmap.Models
{
    public class SearchModel
    {
        public string Login { get; set; }
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public byte[] Avatar { get; set; }

        public EntityType Type { get; set; }

        //для музыканта
        public InstrumentType Instrument { get; set; }
        public int Expirience { get; set; }
    }
}
