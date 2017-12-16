using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Objects
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
