namespace Funkmap.Domain.Models
{
    public class Shop : Profile
    {
        public Shop()
        {
            EntityType = EntityType.Shop;
        }
        public string StoreName { get; set; }
        public string Website { get; set; }

        public string WorkingHoursDescription { get; set; }
    }
}
