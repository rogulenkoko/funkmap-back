namespace Funkmap.Models
{
    public class ShopModel
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string WebSite { get; set; }
        public string Description { get; set; }
        public string VkLink { get; set; }
        public string YouTubeLink { get; set; }
        public string FacebookLink { get; set; }

        public string SoundCloudLink { get; set; }

        public string WorkingHoursDescription { get; set; }
    }

    public class ShopPreviewModel
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public string Address { get; set; }
        public byte[] Avatar { get; set; }
        public string WorkingHoursDescription { get; set; }
        public string VkLink { get; set; }
        public string YouTubeLink { get; set; }
        public string FacebookLink { get; set; }
        public string SoundCloudLink { get; set; }
    }
}
