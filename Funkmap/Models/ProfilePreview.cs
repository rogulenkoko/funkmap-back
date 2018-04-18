using Funkmap.Domain;

namespace Funkmap.Models
{
    public abstract class ProfilePreview
    {
        public EntityType EntityType { get; set; }
        public string Login { get; set; }

        public string UserLogin { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }
        
        public string AvatarMiniId { get; set; }

        public string VkLink { get; set; }
        public string YoutubeLink { get; set; }
        public string FacebookLink { get; set; }
        public string SoundCloudLink { get; set; }
    }
}
