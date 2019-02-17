using System;
using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class Musician : Profile
    {
        public Musician()
        {
            EntityType = EntityType.Musician;
        }
        public Sex? Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public List<Styles> Styles { get; set; }
        
        public Instruments Instrument { get; set; }

        public Experiences Experience { get; set; }

        public List<string> BandLogins { get; set; }

       
    }
}
