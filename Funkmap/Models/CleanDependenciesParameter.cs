using Funkmap.Data.Entities;

namespace Funkmap.Models
{
    public class CleanDependenciesParameter
    {
        public EntityType EntityType { get; set; }
        public string EntityLogin { get; set; }
        public string FromEntityLogin { get; set; }
    }
}
