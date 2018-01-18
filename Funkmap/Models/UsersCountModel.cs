using System.Collections.Generic;
using System.Linq;

namespace Funkmap.Models
{
    public class UsersCountModel
    {
        public UsersCountModel()
        {
            
        }

        public UsersCountModel(ICollection<UsersEntitiesCountModel> counts)
        {
            Counts = counts;
            TotalCount = counts.Select(x=>x.Count).Aggregate((x,y)=>x + y);
        }

        public int TotalCount { get; set; }

        public ICollection<UsersEntitiesCountModel> Counts { get; set; }
    }
}
