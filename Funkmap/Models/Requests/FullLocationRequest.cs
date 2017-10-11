
using Microsoft.Build.Framework;

namespace Funkmap.Models.Requests
{
    public class FullLocationRequest
    {
        [Required]
        public double? RadiusDeg { get; set; }

        [Required]
        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        public double? Latitude { get; set; }

        [Required]
        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        public double? Longitude { get; set; }

        [Required]
        public int Skip { get; set; }

        [Required]
        public int Take { get; set; }
    }
}
