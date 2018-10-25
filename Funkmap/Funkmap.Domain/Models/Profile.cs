using System.Collections.Generic;
using Newtonsoft.Json;

namespace Funkmap.Domain.Models
{
    public abstract class Profile : IHasAvatar
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

        [JsonProperty("AvatarId")] //todo убрать, когда будет все обновлено на фронте
        public string AvatarUrl { get; set; }

        [JsonProperty("AvatarMiniId")]
        public string AvatarMiniUrl { get; set; }

        public bool IsPriority { get; set; }
        
    }
}
