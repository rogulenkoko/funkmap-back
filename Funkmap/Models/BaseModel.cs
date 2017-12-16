using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Microsoft.Build.Framework;

namespace Funkmap.Models
{
    public class BaseModel
    {
        [Required]
        public EntityType EntityType { get; set; }

        [Required]
        public string Login { get; set; }

        public string UserLogin { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string AvatarId { get; set; }
        public string AvatarMiniId { get; set; }
        public List<VideoInfo> VideoInfos { get; set; }

        public string VkLink { get; set; }
        public string YoutubeLink { get; set; }
        public string FacebookLink { get; set; }
        public string SoundCloudLink { get; set; }

        public bool? IsActive { get; set; }
    }
}
