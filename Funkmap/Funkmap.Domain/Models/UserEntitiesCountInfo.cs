using System.Collections.Generic;

namespace Funkmap.Domain.Models
{
    public class UserEntitiesCountInfo
    {
        public UserEntitiesCountInfo()
        {
            Logins = new List<string>();
        }

        public EntityType EntityType { get; set; }
        public int Count { get; set; }

        public List<string> Logins { get; set; }
    }
}
