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
        public ModelType ModelType { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public enum ModelType
    {
        Musician = 1,
        Shop = 2
    }
}
