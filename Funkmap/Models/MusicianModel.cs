using System;
using Funkmap.Data.Entities;
using Microsoft.Build.Framework;

namespace Funkmap.Models
{
    public class MusicianModel : BaseModel
    {
        public Sex? Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public Styles[] Styles { get; set; }

        [Required]
        public InstrumentType Instrument { get; set; }

        public ExpirienceType Expirience { get; set; }

       
    }

    public class MusicianPreviewModel : BaseModel
    {
        public ExpirienceType Expirience { get; set; }
        public Styles[] Styles { get; set; } 
        public string YouTubeLink { get; set; }
        public InstrumentType Instrument { get; set; }
    }
}
