namespace Funkmap.Domain.Models
{
    public class RehearsalPoint : Profile
    {
        public RehearsalPoint()
        {
            EntityType = EntityType.RehearsalPoint;
        }
        public string WorkingHoursDescription { get; set; }
    }
}
