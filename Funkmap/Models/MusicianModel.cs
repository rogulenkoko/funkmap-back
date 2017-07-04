using System;
using Funkmap.Data.Entities;
using Microsoft.Build.Framework;

namespace Funkmap.Models
{
    public class MusicianModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public Styles[] Styles { get; set; }

        [Required]
        public InstrumentType Instrument { get; set; }

        public byte[] Avatar { get; set; }

        public string VkLink { get; set; }
        public string YouTubeLink { get; set; }
        public string FacebookLink { get; set; }
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
    }
}
