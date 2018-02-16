using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Models
{
    public class SearchModel
    {
        public string Login { get; set; }
        public string UserLogin { get; set; }
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AvatarId { get; set; }

        public EntityType Type { get; set; }

        //для музыканта
        public InstrumentType Instrument { get; set; }
        public ExpirienceType Expirience { get; set; }


        //для магазина 
        public string Address { get; set; }
        public string Website { get; set; }

        //для группы 
        public List<Styles> Styles { get; set; }
    }
}
