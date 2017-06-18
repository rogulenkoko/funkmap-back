using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Data.Parameters
{
    public class LocationParameter
    {
        public double RadiusDeg { get; set; }

        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        public double Longitude { get; set; }
    }
}
