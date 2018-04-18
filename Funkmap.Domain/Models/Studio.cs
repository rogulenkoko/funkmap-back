namespace Funkmap.Domain.Models
{
    public class Studio : Profile
    {
        public Studio()
        {
            EntityType = EntityType.Studio;
        }
        public string WorkingHoursDescription { get; set; }
    }
}
