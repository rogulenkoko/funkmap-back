using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain;

namespace Funkmap.Data.Entities.Entities
{
    public class RehearsalPointEntity : EstablishmentEntity
    {
        public RehearsalPointEntity()
        {
            EntityType = EntityType.RehearsalPoint;
        }
    }
}
