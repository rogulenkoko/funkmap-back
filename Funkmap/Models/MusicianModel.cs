using System;
using System.Collections.Generic;
using Funkmap.Data.Entities.Entities;
using Microsoft.Build.Framework;

namespace Funkmap.Models
{
    public class MusicianModel : BaseModel
    {
        public Sex Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public List<Styles> Styles { get; set; }

        [Required]
        public InstrumentType Instrument { get; set; }

        public ExpirienceType Expirience { get; set; }

        public ICollection<string> BandLogins { get; set; }

       
    }

    public class MusicianPreviewModel : BaseModel
    {
        public ExpirienceType Expirience { get; set; }
        public List<Styles> Styles { get; set; } 
        public string YouTubeLink { get; set; }
        public InstrumentType Instrument { get; set; }
    }
}
