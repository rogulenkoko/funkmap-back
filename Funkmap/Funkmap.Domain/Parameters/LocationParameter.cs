using Funkmap.Domain.Abstract;

namespace Funkmap.Domain.Parameters
{
    public class LocationParameter : ILocationParameter
    {
        public LocationParameter()
        {
            Skip = 0;
            Take = 10;
        }

        /// <summary>
        /// Радиус в километрах
        /// </summary>
        public double? RadiusKm { get; set; }

        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        public double? Longitude { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
