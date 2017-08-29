using System;
using System.Collections.Generic;
using Funkmap.Data.Entities;
using Microsoft.Build.Framework;

namespace Funkmap.Models
{
    public class MusicianModel : BaseModel
    {
        public Sex Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public Styles[] Styles { get; set; }

        [Required]
        public InstrumentType Instrument { get; set; }

        public ExpirienceType Expirience { get; set; }

       

        public List<string> VideosYoutube { get; set; }

       
    }

    public class MusicianPreviewModel
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public ExpirienceType Expirience { get; set; }
        public Styles[] Styles { get; set; } 

        public byte[] Avatar { get; set; }

        public string VkLink { get; set; }
        public string YouTubeLink { get; set; }
        public string FacebookLink { get; set; }

        public string SoundCloudLink { get; set; }
    }
}
