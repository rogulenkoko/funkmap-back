using System;
using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class Profile
    {
        public EntityType EntityType { get; set; }
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

        public List<AudioInfo> SoundCloudTracks { get; set; }

        public string VkLink { get; set; }
        public string YoutubeLink { get; set; }
        public string FacebookLink { get; set; }
        public string SoundCloudLink { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VideoInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VideoType Type { get; set; }
        public DateTime SaveDateUtc { get; set; }
    }

    public class AudioInfo
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }
    }
}
