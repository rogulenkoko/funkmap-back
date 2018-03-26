using System;
using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class Musician : Profile
    {
        public Sex Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public List<Styles> Styles { get; set; }
        
        public Instruments Instrument { get; set; }

        public Expiriences Expirience { get; set; }

        public ICollection<string> BandLogins { get; set; }

       
    }
}
