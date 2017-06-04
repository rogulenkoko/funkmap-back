using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Common.Abstract.Search
{
    public abstract class SearchModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public EntityType ModelType { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
