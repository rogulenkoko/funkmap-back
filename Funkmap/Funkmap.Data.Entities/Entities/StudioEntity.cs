using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain;

namespace Funkmap.Data.Entities.Entities
{
    public class StudioEntity : EstablishmentEntity
    {
        public StudioEntity()
        {
            EntityType = EntityType.Studio;
        }
    }
}
