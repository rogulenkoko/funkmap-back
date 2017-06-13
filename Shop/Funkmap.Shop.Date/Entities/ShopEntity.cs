using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Abstract;

namespace Funkmap.Shop.Data.Entities
{
    public class ShopEntity : Entity
    {
        public String StoreName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public String URL { get; set; }
    }
}
