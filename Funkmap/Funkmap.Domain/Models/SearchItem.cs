using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class SearchItem
    {
        public string Login { get; set; }
        public string UserLogin { get; set; }
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AvatarId { get; set; }

        public EntityType Type { get; set; }

        /// <summary>
        ///  Musicial instrument (musician specific)
        /// </summary>
        public Instruments Instrument { get; set; }

        /// <summary>
        /// Musician expirience (musician specific)
        /// </summary>
        public Expiriences Expirience { get; set; }


        /// <summary>
        /// Address (shop specific)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Website (shop specific)
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Musician styles (band specific)
        /// </summary>
        public List<Styles> Styles { get; set; }
    }
}
