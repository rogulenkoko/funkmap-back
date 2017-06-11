
namespace Funkmap.Module.Search.Models
{
    public class NearestRequest
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
