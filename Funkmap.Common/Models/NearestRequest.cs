
using System.ComponentModel.DataAnnotations;

namespace Funkmap.Module.Search.Models
{
    public class NearestRequest
    {
        [Required]
        public double RadiusDeg { get; set; }

        [Required]
        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        public double Latitude { get; set; }

        [Required]
        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        public double Longitude { get; set; }
    }
}
