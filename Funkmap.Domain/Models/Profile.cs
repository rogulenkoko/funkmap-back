using System.Collections.Generic;

namespace Funkmap.Domain.Models
{
    public abstract class Profile
    {
        public EntityType EntityType { get; set; }
        
        public string Login { get; set; }
        public string UserLogin { get; set; }


        #region Editable

        public Location Location { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<VideoInfo> VideoInfos { get; set; }

        public List<AudioInfo> SoundCloudTracks { get; set; }

        public string VkLink { get; set; }
        public string YoutubeLink { get; set; }
        public string FacebookLink { get; set; }
        public string SoundCloudLink { get; set; }

        public bool? IsActive { get; set; }

        #endregion


        public string AvatarId { get; set; }
        public string AvatarMiniId { get; set; }
        
    }
}
