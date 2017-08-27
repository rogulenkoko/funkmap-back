namespace Funkmap.Models
{
    public class ShopModel : BaseModel
    {
        public string StoreName { get; set; }
        public string WebSite { get; set; }

        public string WorkingHoursDescription { get; set; }
    }

    public class ShopPreviewModel : BaseModel
    {
        public string WebSite { get; set; }
        public string WorkingHoursDescription { get; set; }
    }
}
