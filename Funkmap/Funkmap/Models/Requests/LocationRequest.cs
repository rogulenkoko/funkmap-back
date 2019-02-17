using System.ComponentModel.DataAnnotations;

namespace Funkmap.Models.Requests
{
    public class LocationRequest
    {
        /// <summary>
        /// Радиус в километрах
        /// </summary>
        public double? RadiusKm { get; set; }
        
        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        [Required]
        public double? Latitude { get; set; }

        
        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        [Required]
        public double? Longitude { get; set; }

        [Required]
        public int Limit { get; set; }
    }
}
